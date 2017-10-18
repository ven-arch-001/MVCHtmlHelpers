using Mvc.Web.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test.Controllers
{
    public class CheckBoxController : Controller
    {
        // GET: CheckBox
        public ActionResult Index()
        {
            CheckBoxVM model = new ViewModel.CheckBoxVM();
            model.CheckBox = true;
            return View(model);
        }




        [HttpPost]
        public ActionResult Submit(CheckBoxVM model)
        {
            return this.Json(model, JsonRequestBehavior.AllowGet);

        }
    }
}