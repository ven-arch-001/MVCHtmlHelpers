using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.Reflection;
using Mvc.Controls.DataTable.Infrastructure;

namespace Mvc.Controls.DataTable
{
    public static class DataTablesHelper
    {
        public static DataTableConfigVm DataTableVm<TController, TResult>(this HtmlHelper html, string id,
            Expression<Func<TController, DataTablesResult<TResult>>> exp, IEnumerable<ColDef> columns = null)
        {
            if (columns == null || !columns.Any())
            {
                columns = typeof(TResult).ColDefs();
            }

            var mi = exp.MethodInfo();
            var controllerName = typeof (TController).Name;
            controllerName = controllerName.Substring(0, controllerName.LastIndexOf("Controller"));
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var ajaxUrl = urlHelper.Action(mi.Name, controllerName);
            var result =  new DataTableConfigVm(id, columns);
            result.AjaxDataUrl = ajaxUrl;
            return result;
        }

        public static DataTableConfigVm DataTableVm(this HtmlHelper html, Type t, string id, string uri)
        {            
            var result = new DataTableConfigVm(id, t.ColDefs());
            result.AjaxDataUrl = uri;
            return result;
        }
        //public static DataTableConfigVm DataTableVm<T>(string id, string uri)
        ////{
        ////    return new DataTableConfigVm(id, uri.ToString(), typeof(T).ColDefs());

        ////}


        //public static DataTableConfigVm DataTableVm<TResult>(this HtmlHelper html, string id, string uri)
        //{
        //    return DataTableVm(html, typeof (TResult), id, uri);
        //}

        //public static DataTableConfigVm DataTableVm(this HtmlHelper html, string id, string ajaxUrl, params string[] columns)
        //{
        //    return new DataTableConfigVm(id, ajaxUrl, columns.Select(c => new ColDef(c, typeof(string))
        //    {

        //    }));
        //}

      
    }
}