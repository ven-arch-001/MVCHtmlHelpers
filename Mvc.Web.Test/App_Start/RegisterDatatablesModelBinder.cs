using Mvc.Controls.DataTable;
using Mvc.Controls.DataTable.Infrastructure;
using Mvc.Web.Test;
using System.Web;
using System.Web.Mvc;


[assembly: PreApplicationStartMethod(typeof(RegisterDataTablesModelBinder), "Start")]

namespace Mvc.Web.Test
{

    public static class RegisterDataTablesModelBinder
    {
        public static void Start()
        {
            if (!ModelBinders.Binders.ContainsKey(typeof (DataTablesParam)))
                ModelBinders.Binders.Add(typeof (DataTablesParam), new DataTablesModelBinder());
        }
    }
}