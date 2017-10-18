using System;
using System.Linq;
using System.Reflection;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.Reflection;
using System.ComponentModel;
using Mvc.Common;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public enum EditControl
    {
        Default,
        TextBox,
        Numeric,
        DropDown,
        Date,
        CheckBox,
        CheckBoxList,
        Label
    }
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
            this.Searchable = false;
            this.Editable = false;
            this.Visible = true;
            this.SortDirection = SortDirection.None;
            this.Order = int.MaxValue;
        }
        public bool Editable { get; set; }
        public EditControl EditControl { get; set; }
        public string EditControlDataUri { get; set; }
        public bool Searchable { get; set; }
        bool? sortable { get; set; }
        public bool Sortable
        {
            get { return this.sortable.HasValue ? this.sortable.Value : false; }
            set
            {
                this.sortable = value;
                if (this.SortDirection == SortDirection.None && value)
                {
                    this.SortDirection = SortDirection.Ascending;
                }
            }
        }
        public int Order { get; set; }
        public string DisplayName { get; set; }
        public Type DisplayNameResourceType { get; set; }
        public SortDirection SortDirection { get; set; }
        public string MRenderFunction { get; set; }
        public String CssClass { get; set; }
        public String CssClassHeader { get; set; }
        public int Width { get; set; } = 10;

        public WidthUnit WidthUnit { get; set; }

        bool visible { get; set; }
        public bool Visible
        {
            get { return this.visible; }
            set
            {
                this.visible = value;
                if (!this.sortable.HasValue)
                {
                    this.sortable = value;
                    this.SortDirection = SortDirection.Ascending;
                }
            }
        }

        public override void ApplyTo(ColDef colDef, System.Reflection.PropertyInfo pi)
        {

            colDef.DisplayName = this.ToDisplayName() ?? colDef.Name.ToTitleCaseFromPascal();
            colDef.Sortable = this.Sortable;
            colDef.Visible = this.Visible;
            colDef.Editable = this.Editable;
            colDef.EditControl = this.EditControl == EditControl.Default ?
                colDef.EditControl : this.EditControl;
            colDef.EditControlDataUri = this.EditControlDataUri;
            colDef.Searchable = this.Searchable;
            colDef.SortDirection = this.SortDirection;
            colDef.MRenderFunction = this.MRenderFunction;
            colDef.CssClass = this.CssClass;
            colDef.CssClassHeader = this.CssClassHeader;
            colDef.CustomAttributes = pi.GetCustomAttributes().ToArray();
            colDef.Width = this.Width.ToString() + this.WidthUnit.Value();











            //Validations
            if (this.EditControl == EditControl.DropDown &&
                string.IsNullOrEmpty(this.EditControlDataUri))
            {
                throw new Exception("Please set the dataUrl for Edit control (Dropdown)");
            }
        }
    }
}