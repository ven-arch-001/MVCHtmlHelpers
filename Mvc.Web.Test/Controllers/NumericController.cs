using Mvc.Web.Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test.Controllers
{
    public class NumericController : Controller
    {
        // GET: Numeric
        public ActionResult Index()
        {
            NumericVM data = new Models.NumericVM();
            data.Height = 6.0m;
            data.Weight = 190;
           return View(data);
        }
    }
}