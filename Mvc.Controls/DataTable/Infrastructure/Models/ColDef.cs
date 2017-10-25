using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Mvc.Controls.DataTable.Infrastructure.Models
{
    public class ColDef
    {
        public ColDef(string name, Type type)
        {
            Name = name;
            Type = type;
            Filter = new FilterDef(Type);
            DisplayName = name;
            Visible = true;
            Sortable = true;
            SortDirection = SortDirection.None;
            MRenderFunction = (string) null;
            CssClass = "";
            CssClassHeader = "";
            this.Searchable = true;
            this.Editable = false;

            //this.AssignEditControl(type);
            this.EditControl = ControlHelpers.AssignEditControl(type);

        }

        //private void AssignEditControl(Type type)
        //{
        //    if (type == null || type.GetTypeInfo().IsEnum)
        //    {
        //        return;
        //    }

        //    switch (Type.GetTypeCode(type))
        //    {

        //        case TypeCode.Decimal:
        //        case TypeCode.Double:
        //        case TypeCode.Single:
        //            this.EditControl = EditControl.Decimal;
        //            break;
        //        case TypeCode.Int16:
        //        case TypeCode.Int32:
        //        case TypeCode.Int64:                
        //        case TypeCode.UInt16:
        //        case TypeCode.UInt32:
        //        case TypeCode.UInt64:
        //            this.EditControl = EditControl.Numeric;
        //            break;
        //        case TypeCode.String:
        //        case TypeCode.Char:
        //            this.EditControl = EditControl.TextBox;
        //            break;
        //        case TypeCode.Boolean:
        //            this.EditControl = EditControl.CheckBox;
        //            break;
        //        case TypeCode.DateTime:
        //            this.EditControl = EditControl.Date;
        //            break;
        //        case TypeCode.Object:
        //            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        //            {
        //                AssignEditControl(Nullable.GetUnderlyingType(type));
        //            }
        //            break;
        //        default:
        //            this.EditControl = EditControl.Default;
        //            break;
        //    }
            
        //}


        public bool Editable { get; set; }
        public EditControl EditControl { get; set; }
        public string EditControlDataUri { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
        public bool Sortable { get; set; }
        public Type Type { get; set; }
        public bool Searchable { get; set; }
        public String CssClass { get; set; }
        public String CssClassHeader { get; set; }
        public SortDirection SortDirection { get; set; }
        public string MRenderFunction { get; set; }
        public FilterDef Filter { get; set; }

        public JObject SearchCols { get; set; }
        public Attribute[] CustomAttributes { get; set; }
        public string Width { get; set; }
    }
}