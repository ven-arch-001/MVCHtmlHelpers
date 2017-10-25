using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class NumericVM
    {    

        [Required]
        [Display(Name = "Weight (Lb)", Description = "Weight Desc")]        
        public int Weight { get; set; }



        [Required]
        [Display(Name = "Height (Ft)", Description = "Height Desc")]
        public decimal Height { get; set; }

    }
}