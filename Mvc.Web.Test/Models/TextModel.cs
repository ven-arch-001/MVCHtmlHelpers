using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class TextModel
    {    

        [Required]
        [Display(Name = "Text Label", Description = "Text Desc")]        
        public string TextBox { get; set; }


        [Display(Name = "Text Area Label", Description = "Text Area Desc")]
        public string TextArea { get; set; }

    }
}