using Mvc.Controls.DataTable.Infrastructure;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.Web.Test.ViewModel
{    
    public class UserVM : IDataTable
    {
        [DataTables(Visible = false)]
        public int UserId { get; set; }

        [DataTables(Width = 25, WidthUnit = WidthUnit.Percent, DisplayName = "Last Name", 
            Sortable = false, SortDirection = SortDirection.None, 
            Searchable = true, Visible = true)]
        //[DataTablesFilter(Selector = "#" + nameof(LastName) + "Filter")]
        public string LastName { get; set; }


        [DataTables(Width = 500, WidthUnit = WidthUnit.Pixel, Visible = true)]
        //[DataTablesFilter(Selector = "#" + nameof(FirstName) + "Filter")]
        public string FirstName { get; set; }

        //[DataTablesFilter(DataTablesFilterType.DateTimeRange, Selector = "#" + nameof(BirthDate) + "Filter")]
        //[DefaultToStartOf2014]
        [DataTables(Visible = true)]
        public DateTime? BirthDate { get; set; }

        [DataTables(Sortable = false, Searchable = true)]
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


        [DataTables(Sortable = false, Searchable = false, Visible = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public int PictureUri { get; set; }

        [DataTables(Sortable = false, Searchable = false)]
        //[DataTablesFilter(DataTablesFilterType.None)]
        public string Content { get; set; }

        public CustomButtonFunction RowFunction
        { get; set; }
    }
}