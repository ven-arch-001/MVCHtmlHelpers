using Mvc.Controls.DataTable.Infrastructure;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Web.Test.ViewModel
{    
    public class UserVM : IDataTable
    {
        [DataTables(Visible = false)]
        public int UserId { get; set; }

        [AllowHtml]
        [DataTables(Width = 25, WidthUnit = WidthUnit.Percent, DisplayName = "Last Name", 
            Sortable = false, SortDirection = SortDirection.None, 
            Searchable = true, Visible = true, Editable = false)]
        //[DataTablesFilter(Selector = "#" + nameof(LastName) + "Filter")]
        public string LastNameTem { get; set; }

        [AllowHtml]
        [DataTables(Width = 500, WidthUnit = WidthUnit.Pixel, Visible = true, Editable = false)]
        //[DataTablesFilter(Selector = "#" + nameof(FirstName) + "Filter")]
        public string FirstNameTem { get; set; }


        [DataTables(Sortable = false, Searchable = false, Visible = false, Editable = true)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string LastName { get; set; }



        [DataTables(Sortable = false, Searchable = false, Visible = false, Editable = true)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string FirstName { get; set; }

      
        [DataTables(Visible = false, Editable = true)]
        public DateTime? BirthDate { get; set; }


        [AllowHtml]
        [DataTables(Visible = true, Editable = false, DisplayName = "Birth Date")]
        public string BirthDateTem { get; set; }

        [DataTables(Sortable = false, Searchable = true, Editable = true)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string SSN { get; set; }


        [DataTables(Sortable = false, Searchable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string Address { get; set; }

        [DataTables(Sortable = false, Searchable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string Gender { get; set; }


        [DataTables(Sortable = false, Searchable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string State { get; set; }


        [DataTables(Sortable = false, Searchable = false, Visible = false, Editable = true)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public bool IsActive { get; set; }

        [AllowHtml]
        [DataTables(Sortable = false, Searchable = false, Visible = true, Editable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string Status { get; set; }


        [DataTables(Sortable = false, Searchable = false, Visible = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public int PictureUri { get; set; }

        [AllowHtml]
        [DataTables(Sortable = false, Searchable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string Content { get; set; }


      


        public CustomButtonFunction RowFunction
        { get; set; }



        [DataTables(Visible = false, Editable = true, DisplayName = "Gender", EditControlDataUri = "MasterData/Gender", EditControl = EditControl.DropDown)]
        public int GenderId { get; set; }


        [DataTables(Visible = false, Editable = true, DisplayName = "State", EditControlDataUri = "MasterData/State", EditControl = EditControl.DropDown)]
        public int StateId { get; set; }

    }
}