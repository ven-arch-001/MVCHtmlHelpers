using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class CascadingDropdownsModel
    {
        //public IList<SelectListItem> Countries { get; set; }

        [Required]
        public string SelectedCountry { get; set; }

        [Required]
        public string SelectedCity { get; set; }

        [Required]
        [Display(Name = "Street Display", Description = "Street Desc")]        
        public int? SelectedStreet { get; set; }

        [Display(Name ="Country")]
        [Required]
        public string SelectedCountryJQ { get; set; }


        [Display(Name = "City")]
        [Required]
        public string SelectedCityJQ { get; set; }


        [Display(Name = "Street")]
        [Required]
        public string SelectedStreetJQ { get; set; }


    }
}