using System;
using System.Linq.Expressions;
using System.Linq;
using Mvc.Common;

namespace Mvc.Controls
{
    public enum ControlAccess
    {
        Edit,
        Read,
        ReadPost,
        Hide
    }


    public abstract class ControlInputBase<TModel>
    {
        /// <summary>
        ///     This will be appended to the Id attribute to make it unique
        /// </summary>
        public string Group { get; set; } = string.Empty;

        public string IdAttr { get; set; } = string.Empty;
        public TModel Value { get; set; }
        public string NameAttr { get; set; } = string.Empty;
        public Expression<Func<TModel, object>> ModelProperty { get; set; } = null;

        public virtual string PlaceHolder { get; set; } = "Enter value";
        public object DataHtmlAttributes { get; set; }
        public string LabelText { get; set; } = string.Empty;
        public object LabelHtmlAttributes { get; set; }
        public bool ShowLabel { get; set; }
        public ControlAccess Access { get; set; } = ControlAccess.Edit;
        public virtual int WidthPerCent { get; set; } = 50;
        public virtual int HeightPixel { get; set; } = 0;
        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(this.NameAttr) ? this.IdAttr : this.NameAttr;
            }

        }

        /// <summary>
        ///  ModelProperty Name is derived from ModelProperty for String data types. For Null data types, it has to be calculated
        /// </summary>
        public string ModelPropertyName
        {
            get
            {
                if (this.ModelProperty == null)
                    return string.Empty;

                string text = this.ModelProperty.ToString();
                text = text.Replace("(", string.Empty).Replace(")", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
                return text.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
        }
    }

    public abstract class ControlInputDataSourceBase<TModel> : ControlInputBase<TModel>
    {
        public override string PlaceHolder { get; set; } = "--Select--";
        public string DataUri { get; set; }
        public string DataUriParamName { get; set; }
    }

    public class CascadingInput<TModel> : ControlInputDataSourceBase<TModel>
    {

        public Expression<Func<TModel, object>> TriggeredByModelProperty { get; set; } = null;
        public string TriggeredById { get; set; } = null;
        public bool DisabledWhenParentNotSelected { get; set; } = true;


    }

    public class DropdownInput<TModel> : ControlInputDataSourceBase<TModel>
    {
        /// <summary>
        ///     Data value passed to the source to load the data
        /// </summary>
        public string DataUriParamValue { get; set; }

    }


    public abstract class CheckInput<TModel> : ControlInputDataSourceBase<TModel>
    {
        public TModel DefaultUnCheckedValue { get; set; }
        public TModel DefaultCheckedValue { get; set; }
    }


    public class CheckBoxStringInput<String> : CheckInput<String>
    {               
        public  new string DefaultUnCheckedValue { get; set; } = "N";
        public new string DefaultCheckedValue { get; set; } = "N";
    }

    public class CheckBoxInput<Boolean> : CheckInput<Boolean>
    {
        public new bool DefaultUnCheckedValue { get; set; } = false;
        public new bool DefaultCheckedValue { get; set; } = true;
    }

    public class DateInput<DateTime> : ControlInputBase<DateTime>
    {
        public override string PlaceHolder { get; set; } = "Select Date";

        public string DateFormat { get; set; } = "MM/dd/yyyy";

        public string DateFormatJS
        {
            get
            {
                string format = string.Empty;
                switch (this.DateFormat)
                {
                    case "MM/dd/yyyy":
                        format = "mm/dd/yy";
                        break;
                }
                return format;
            }
        }
    }



    public class TextInput<String> : ControlInputBase<String>
    {
        public override string PlaceHolder { get; set; } = "Enter value";

        public int Rows { get; set; } = 1;

    }



    public class NumericTextInput<T> : ControlInputBase<T>
    {
       public override string PlaceHolder { get; set; } = "Enter value";

        public NumericType Type { get; set; } = NumericType.Default;

        public override int WidthPerCent { get; set; } = 25;
    }


    public enum NumericType
    {
        Default, 
        Int,
        Decimal
    }
}













 