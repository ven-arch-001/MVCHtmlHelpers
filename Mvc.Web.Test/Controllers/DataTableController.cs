using Mvc.Controls.DataTable;
using Mvc.Controls.DataTable.Infrastructure;
using Mvc.Web.Test.Models.EF;
using Mvc.Web.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Mvc.Web.Test.Controllers
{
    public class DataTableController : Controller
    {
        // GET: DataTable
        public ActionResult Index()
        {
            return View();
        }



        public DataTablesResult<UserVM> GetUsersInclude(DataTablesParam dataTableParam)
        {
            TestDB context = new Models.EF.TestDB();

            var data = context.Users
                .Select(user => new UserVM()
                {
                    UserId = user.UserId,
                    LastName = user.LastName,
                    FirstName = user.FirstName,  
                    IsActive = user.IsActive,
                    GenderId = user.GenderId,
                    StateId = user.StateId,
                    Address = user.Address,
                    SSN = user.SSN,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender.Text,
                    State = user.State.Text,
                    //PictureUri = "https://randomuser.me/api/portraits/thumb/men/" + user.UserId + ".jpg"
                    PictureUri = user.UserId
                }).AsQueryable();

            return DataTablesResult.Create(data, dataTableParam,
                rowViewModel => new
                {
                    LastNameTem = string.Format("<b>{0}</b>", rowViewModel.LastName),
                    FirstNameTem = string.Format("<b><u>{0}</u></b>", rowViewModel.FirstName),
                    Content = "<div>" +
                              "  <img src='" + "https://randomuser.me/api/portraits/thumb/men/" + (rowViewModel.PictureUri % 100) + ".jpg" + "' />" +
                              "</div>",
                    BirthDateTem = rowViewModel.BirthDate.HasValue ? rowViewModel.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,

                });
        }



        [HttpPost]
        public DataTablesResult<UserVM> GetUsersInnerJoin(DataTablesParam dataTableParam)
        {
            TestDB context = new Models.EF.TestDB();
            var repUser = context.Users.AsQueryable();
            var repState = context.States.AsQueryable();
            var repGender = context.Genders.AsQueryable();
            var data = from a in repUser
                       join b in repState on a.StateId equals b.StateId
                       join c in repGender on a.GenderId equals c.GenderId
                       select new UserVM()
                       {
                           UserId = a.UserId,
                           SSN = a.SSN,
                           LastName = a.LastName,
                           FirstName = a.FirstName,
                           IsActive = a.IsActive,
                           Address = a.Address,
                           GenderId = a.GenderId,
                           StateId = a.StateId,
                           BirthDate = a.BirthDate,
                           Gender = c.Text,
                           State = b.Text,
                           PictureUri = a.UserId
                       };

            return DataTablesResult.Create(data, dataTableParam,
                rowViewModel => new
                {
                    LastNameTem = string.Format("<b>{0}</b>", rowViewModel.LastName),
                    FirstNameTem = string.Format("<b><u>{0}</u></b>", rowViewModel.FirstName),
                    Content = "<div>" +
                              "  <img src='" + "https://randomuser.me/api/portraits/thumb/men/" + (rowViewModel.PictureUri % 100) + ".jpg" + "' />" +
                              "</div>",
                    BirthDateTem = rowViewModel.BirthDate.HasValue ? rowViewModel.BirthDate.Value.ToString("dd-MMM-yyyy") : string.Empty,

                });
        }



        [HttpPost]
        public DataTablesResult<UserVM> GetUsersOuterJoin(DataTablesParam dataTableParam)
        {
            TestDB context = new Models.EF.TestDB();
            var repUser = context.Users.AsQueryable();
            var repState = context.States.AsQueryable();
            var repGender = context.Genders.AsQueryable();
            var data = from a in repUser
                       join b in repState on a.StateId equals b.StateId
                       into tempState
                       from b in tempState.DefaultIfEmpty()
                       join c in repGender on a.GenderId equals c.GenderId
                       into tempGender
                       from c in tempGender.DefaultIfEmpty()
                       select new UserVM()
                       {
                           UserId = a.UserId,
                           SSN = a.SSN,
                           LastName = a.LastName,
                           FirstName = a.FirstName,
                           IsActive = a.IsActive,
                           GenderId = a.GenderId,
                           StateId = a.StateId,
                           Address = a.Address,

                           BirthDate = a.BirthDate,
                           Gender = c.Text,
                           State = b.Text,
                           PictureUri = a.UserId
                       };

            return DataTablesResult.Create(data, dataTableParam,
                rowViewModel => new
                {
                    LastNameTem = string.Format("<b>{0}</b>", rowViewModel.LastName),
                    FirstNameTem = string.Format("<b><u>{0}</u></b>", rowViewModel.FirstName),
                    Content = "<div>" +
                              "  <img src='" + "https://randomuser.me/api/portraits/thumb/men/" + (rowViewModel.PictureUri % 100) + ".jpg" + "' />" +
                              "</div>",
                    BirthDateTem = rowViewModel.BirthDate.HasValue ? rowViewModel.BirthDate.Value.ToString("dd-MMM-yy") : string.Empty,

                });
        }



        [HttpPost]
        public JsonResult PostData(UserVM data)
        {
            var db = new TestDB();
            User dataEF = new Models.EF.User();
            AutoMapperServiceConfig.Mapper.Map<UserVM, User>(data, dataEF);



            switch (data.RowFunction)
            {                
                case CustomButtonFunction.Add:
                    db.Users.Add(dataEF);
                    db.Entry(dataEF).State = System.Data.Entity.EntityState.Added;
                    break;
                case CustomButtonFunction.Edit:
                    db.Users.Add(dataEF);
                    db.Entry(dataEF).State = System.Data.Entity.EntityState.Modified;
                    break;
                case CustomButtonFunction.Delete:
                    dataEF.IsActive = false;
                    db.Users.Add(dataEF);
                    db.Entry(dataEF).State = System.Data.Entity.EntityState.Modified;
                    break;
                case CustomButtonFunction.Custom:
                    throw new NotImplementedException("Custom not implmented");
                    
                default:
                    throw new Exception("Row Function not set");
                  
            }
            db.SaveChanges();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}