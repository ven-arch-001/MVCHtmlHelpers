using System;
using System.Reflection;
using Mvc.Controls.DataTable.Infrastructure.Models;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public abstract class DataTablesAttributeBase : Attribute
    {
        public abstract void ApplyTo(ColDef colDef, System.Reflection.PropertyInfo pi);
    }
}