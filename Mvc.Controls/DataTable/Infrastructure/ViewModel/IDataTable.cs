using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Controls.DataTable.Infrastructure.ViewModel
{
    public interface IDataTable
    {
        CustomButtonFunction RowFunction { get; set; }
    }
}
