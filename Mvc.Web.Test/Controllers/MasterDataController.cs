using Mvc.Controls.DataTable;
using Mvc.Controls.DataTable.Infrastructure;
using Mvc.Web.Test.Models.EF;
using Mvc.Web.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test.Controllers
{
    public class MasterDataController : Controller
    {
        public ActionResult Gender()
        {
            TestDB context = new Models.EF.TestDB();
            var gender = context.Genders.AsEnumerable();
            var genderVM = new List<SelectListItem>();
            genderVM = AutoMapperServiceConfig.Mapper.Map<List<SelectListItem>>(gender);            
            return Json(genderVM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult State()
        {
            TestDB context = new Models.EF.TestDB();
            var state = context.States.AsEnumerable();
            var stateVM = new List<SelectListItem>();
            stateVM = AutoMapperServiceConfig.Mapper.Map<List<SelectListItem>>(state);
            return Json(stateVM, JsonRequestBehavior.AllowGet);
        }
    }
}