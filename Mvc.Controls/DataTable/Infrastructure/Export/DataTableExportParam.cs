using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public class DataTableExportParam
    {
        public DataTablesParam dataTableParam { get; set; }

        public string DataUri { get; set; }

        public string DataController { get; set; }

        public string DataAction { get; set; }

        public string Format { get; set; } = "excel";
    }
}
