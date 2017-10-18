using System;
using System.Linq;
using System.Reflection;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.Reflection;
using System.ComponentModel;
using Mvc.Common;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public enum WidthUnit
    {
        [Value(Data = "%")]
        Percent,
        [Value(Data = "px")]
        Pixel,
        [Value("em")]
        em
    }
    public class DataTablesAttribute : DataTablesAttributeBase
    {
        public DataTablesAttribute()
        {
            this.Sortable = true;
            this.Searchable = true;
            this.Visible = true;
            this.Order = int.MaxValue;
        }

        public bool Searchable { get; set; }
        public bool Sortable { get; set; }
        public int Order { get; set; }
        public string DisplayName { get; set; }
        public Type DisplayNameResourceType { get; set; }
        public SortDirection SortDirection { get; set; }
        public string MRenderFunction { get; set; }
        public String CssClass { get; set; }
        public String CssClassHeader { get; set; }
        public int Width { get; set; } = 10;

        public WidthUnit WidthUnit { get; set; }

        public bool Visible { get; set; }

        public override void ApplyTo(ColDef colDef, System.Reflection.PropertyInfo pi)
        {
            
            colDef.DisplayName = this.ToDisplayName() ?? colDef.Name;
            colDef.Sortable = this.Sortable;
            colDef.Visible = this.Visible;
            colDef.Searchable = this.Searchable;
            colDef.SortDirection = this.SortDirection;
            colDef.MRenderFunction = this.MRenderFunction;
            colDef.CssClass = this.CssClass;
            colDef.CssClassHeader = this.CssClassHeader;
            colDef.CustomAttributes = pi.GetCustomAttributes().ToArray();
            colDef.Width = this.Width.ToString() + this.WidthUnit.Value();
        }
    }
}