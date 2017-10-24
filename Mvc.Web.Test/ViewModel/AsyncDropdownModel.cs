using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class AsyncDropdownModel
    {    

        [Required]
        [Display(Name = "Country Name", Description = "Country Desc")]        
        public string SelectedCountry { get; set; }



        [Required]
        [Display(Name = "Country Name JQ", Description = "Country Desc")]
        public string SelectedCountryJQ { get; set; }



        [Required]
        [Display(Name = "States JQ", Description = "State Desc")]
        public string States { get; set; }

    }
}