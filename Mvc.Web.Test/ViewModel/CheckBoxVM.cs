using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.ViewModel
{
    public class CheckBoxVM
    {
        [Display(Name = "Visible?", Description = "Visible Desc")]
        public bool CheckBox { get; set; }
        public string CheckBoxList { get; set; }
    }
}