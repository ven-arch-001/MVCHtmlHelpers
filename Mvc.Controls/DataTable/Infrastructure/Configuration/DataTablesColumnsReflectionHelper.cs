using System;
using System.Collections.Generic;
using System.Linq;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.Reflection;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public static class DataTablesColumnsReflectionHelper{
        public static IEnumerable<ColDef> ColDefs (this Type t)
        {
            var propInfos = DataTablesTypeInfo.Properties(t);
            var columnList = new List<ColDef>();
            
            foreach (var dtpi in propInfos)
            {

                var colDef = new ColDef(dtpi.PropertyInfo.Name, dtpi.PropertyInfo.PropertyType);
                foreach (var att in dtpi.Attributes)
                {
                    att.ApplyTo(colDef, dtpi.PropertyInfo);
                }
                
                columnList.Add(colDef);
            }
            return columnList.ToArray();
        }
        public static IEnumerable<ColDef> ColDefs<TResult>()
        {
            return ColDefs(typeof(TResult));
        }

     
    }
}