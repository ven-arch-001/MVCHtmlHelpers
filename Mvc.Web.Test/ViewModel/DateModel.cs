using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DateModel
    {    

        [Required]
        [Display(Name = "Date of Birth", Description = "Birth Desc")]        
        public DateTime? SelectedDate { get; set; } 

    }
}