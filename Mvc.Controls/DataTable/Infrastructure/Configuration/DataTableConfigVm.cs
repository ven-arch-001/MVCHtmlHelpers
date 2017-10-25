using System;
using System.Collections.Generic;
using System.Linq;
using Mvc.Controls.DataTable.Infrastructure.Models;
using Mvc.Controls.DataTable.Infrastructure.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Mvc.Controls.DataTable.Infrastructure
{
    public enum CustomButtonFunction
    {
        None,
        View,
        Edit,
        Add,
        Delete,
        Custom
    }

    public class CustomButton
    {
        public bool Show { get; set; } = true;
        public string HeaderText { get; set; }
        public string LabelText { get; set; }
        public string CallBackEvent { get; set; } = string.Empty;
        public string CustomPopupId { get; set; } = string.Empty;
        public CustomButtonFunction Functionality { get; set; } = CustomButtonFunction.None;
        public string Glyphicon { get; set; } = string.Empty;


        private string clickEvent = string.Empty;
        public void SetClickEvent(string eventName)
        {
            this.clickEvent = eventName;
        }

        public string GetClickEvent()
        {
            return this.clickEvent;
        }


        private string dataArg = string.Empty;
        public void SetEventForDataArg(string eventName)
        {
            this.dataArg = eventName;
        }

        public string GetEventForDataArg()
        {
            return this.dataArg;
        }

    }


    public class DataTableConfigVm
    {
        public bool HideHeaders { get; set; }
        IDictionary<string, object> m_JsOptions = new Dictionary<string, object>();

        static DataTableConfigVm()
        {
            //DefaultTableClass = "table table-bordered table-striped";
            DefaultTableClass = "table table-striped table-bordered dataTable no-footer";
        }

        public static string DefaultTableClass { get; set; }
        public string TableClass { get; set; }

        public DataTableConfigVm(string id, IEnumerable<ColDef> columns)
        {

            this.Id = id;
            this.Columns = columns;
            this.Filter = true;

            this.ShowPageSizes = true;
            this.TableTools = true;
            ColumnFilterVm = new ColumnFilterSettingsVm(this);
            this.AjaxErrorHandler =
                @"function(jqXHR, textStatus, errorThrown)" +
                "{ " +
                    "console.log('Error in the data table: ' + textStatus + ' - ' + errorThrown); " +
                    "console.log(arguments);" +
                    "alert('Error in the data table: ' + textStatus + ' - ' + errorThrown, 'error');" +
                "}";
        }

        /// <summary>
        /// Enables searching of columns
        /// </summary>
        public bool Filter { get; set; }

        public string Id { get; set; }



        public string ViewClick { get { return "view" + this.Id.Replace("-", string.Empty); } }

        public string SaveClick { get { return "save" + this.Id.Replace("-", string.Empty); } }

        public string EditClick { get { return "edit" + this.Id.Replace("-", string.Empty); } }

        public string EditDataRecord { get { return "data" + this.Id.Replace("-", string.Empty); } }

        //public string AddClick { get { return "add" + this.Id.Replace("-", string.Empty); } }

        public string AddClickEvent
        {
            get
            {

                var button = this.CustomButtons.Where(t => t.Functionality == CustomButtonFunction.Add).FirstOrDefault();
                if (button != null)
                {
                    string text = !string.IsNullOrEmpty(button.LabelText) ? button.LabelText :
              string.Format("<span class=\"{0}\"></span>", button.Glyphicon);
                    string clickEvent = string.IsNullOrEmpty(button.GetClickEvent()) ?
                        string.Empty :
                        button.GetClickEvent();
                    string callbackEvent =
                        string.IsNullOrEmpty(button.CallBackEvent) ? string.Empty :
                        string.Format("{0}({1}());", button.CallBackEvent, button.GetEventForDataArg());
                    return string.Format("<button class='btn btn-primary' onclick=\"{0};{1};\">{2}</button>", clickEvent, callbackEvent, text);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string DeleteClick { get { return "del" + this.Id.Replace("-", string.Empty); } }

        public string DataArgFetch { get { return "cus" + this.Id.Replace("-", string.Empty); } }

        public string DataArgFetchCall { get { return this.DataArgFetch + "()"; } }

        public string AjaxDataUrl { get; set; }

        public string AjaxPostUrl { get; set; }

        public string DataController { get; set; }

        public string DataAction { get; set; }

        public IEnumerable<ColDef> Columns { get; set; }

        public IEnumerable<ColDef> ColumnsEdit
        {
            get
            {
                return this.Columns.Where(t => t.Editable).AsEnumerable();
            }
        }

        public IDictionary<string, object> JsOptions { get { return m_JsOptions; } }

        public bool PrintScript { get; set; } = false;

        public string ColumnDefsString
        {
            get
            {
                return ConvertColumnDefsToJson(this.Columns, this.CustomButtons);
            }
        }

        public string ColumnsString
        {
            get
            {
                return ConvertColumnsToJson(this.Columns);
            }
        }

        public bool UseColumnFilterPlugin { get; set; }

        public ColumnFilterSettingsVm ColumnFilterVm { get; set; }

        public bool TableTools { get; set; }

        public bool AutoWidth { get; set; }

        public List<CustomButton> CustomButtons { get; set; } = new List<CustomButton>();


        public JToken SearchCols
        {
            get
            {
                var initialSearches = Columns
                    .Select(c => c.Searchable & c.SearchCols != null ?
                    c.SearchCols : null as object).ToArray();
                return new JArray(initialSearches);
            }
        }



        private string _dom;

        public string Dom
        {
            get
            {
                if (!string.IsNullOrEmpty(_dom))
                    return _dom;

                string str = "";
                if (this.ShowVisibleColumnPicker)
                    str += "C";
                if (this.TableTools)
                    str += "T<\"clear\">";
                if (this.ShowPageSizes)
                    str += "l";
                if (this.ShowFilterInput)
                    str += "f";
                return str + "tipr";
            }

            set { _dom = value; }
        }


        public bool ShowVisibleColumnPicker { get; set; }

        public bool ShowFilterInput { get; set; }

        //[Obsolete("Use .Filter and .ShowFilterInput")]
        //public bool ShowSearch
        //{
        //    get { return ShowFilterInput && Filter; }
        //    set
        //    {
        //        ShowFilterInput = value;
        //        Filter = value;
        //    }
        //}

        public bool ColVis { get; set; }

        public string ColumnSortingString
        {
            get
            {
                return ConvertColumnSortingToJson(Columns);
            }
        }

        public bool ShowPageSizes { get; set; }

        public bool StateSave { get; set; }

        public string Language { get; set; }

        public string DrawCallback { get; set; }
        public LengthMenuVm LengthMenu { get; set; }
        public int? PageLength { get; set; }
        public string GlobalJsVariableName { get; set; }

        //private bool _columnFilter;

        public bool FixedLayout { get; set; }
        public string AjaxErrorHandler { get; set; }

        public class _FilterOn<TTarget>
        {
            private readonly TTarget _target;
            private readonly ColDef _colDef;

            public _FilterOn(TTarget target, ColDef colDef)
            {
                _target = target;
                _colDef = colDef;

            }

            public TTarget Select(params string[] options)
            {
                _colDef.Filter.type = "select";
                _colDef.Filter.values = options.Cast<object>().ToArray();
                if (_colDef.Type.GetTypeInfo().IsEnum)
                {
                    _colDef.Filter.values = _colDef.Type.EnumValLabPairs();
                }
                return _target;
            }
            public TTarget NumberRange()
            {
                _colDef.Filter.type = "number-range";
                return _target;
            }

            public TTarget DateRange()
            {
                _colDef.Filter.type = "date-range";
                return _target;
            }

            public TTarget Number()
            {
                _colDef.Filter.type = "number";
                return _target;
            }

            public TTarget CheckBoxes(params string[] options)
            {
                _colDef.Filter.type = "checkbox";
                _colDef.Filter.values = options.Cast<object>().ToArray();
                if (_colDef.Type.GetTypeInfo().IsEnum)
                {
                    _colDef.Filter.values = _colDef.Type.EnumValLabPairs();
                }
                return _target;
            }

            public TTarget Text()
            {
                _colDef.Filter.type = "text";
                return _target;
            }

            public TTarget None()
            {
                _colDef.Filter = null;
                return _target;
            }
        }
        public _FilterOn<DataTableConfigVm> FilterOn<T>()
        {
            return FilterOn<T>(null);
        }
        public _FilterOn<DataTableConfigVm> FilterOn<T>(object jsOptions)
        {
            IDictionary<string, object> optionsDict = DataTableConfigVm.ConvertObjectToDictionary(jsOptions);
            return FilterOn<T>(optionsDict);
        }
        ////public _FilterOn<DataTableConfigVm> FilterOn<T>(IDictionary<string, object> filterOptions)
        ////{
        ////    return new _FilterOn<DataTableConfigVm>(this, this.FilterTypeRules, (c, t) => t == typeof(T), filterOptions);
        ////}
        public _FilterOn<DataTableConfigVm> FilterOn(string columnName)
        {
            return FilterOn(columnName, null);
        }
        public _FilterOn<DataTableConfigVm> FilterOn(string columnName, object jsOptions)
        {
            IDictionary<string, object> optionsDict = ConvertObjectToDictionary(jsOptions);
            return FilterOn(columnName, optionsDict);
        }
        public _FilterOn<DataTableConfigVm> FilterOn(string columnName, object jsOptions, object jsInitialSearchCols)
        {
            IDictionary<string, object> optionsDict = ConvertObjectToDictionary(jsOptions);
            IDictionary<string, object> initialSearchColsDict = ConvertObjectToDictionary(jsInitialSearchCols);
            return FilterOn(columnName, optionsDict, initialSearchColsDict);
        }
        public _FilterOn<DataTableConfigVm> FilterOn(string columnName, IDictionary<string, object> filterOptions)
        {
            return FilterOn(columnName, filterOptions, null);
        }
        public _FilterOn<DataTableConfigVm> FilterOn(string columnName, IDictionary<string, object> filterOptions, IDictionary<string, object> jsInitialSearchCols)
        {
            var colDef = this.Columns.Single(c => c.Name == columnName);
            if (filterOptions != null)
            {
                foreach (var jsOption in filterOptions)
                {
                    colDef.Filter[jsOption.Key] = jsOption.Value;
                }
            }
            if (jsInitialSearchCols != null)
            {
                colDef.SearchCols = new JObject();
                foreach (var jsInitialSearchCol in jsInitialSearchCols)
                {
                    colDef.SearchCols[jsInitialSearchCol.Key] = new JValue(jsInitialSearchCol.Value);
                }
            }
            return new _FilterOn<DataTableConfigVm>(this, colDef);
        }

        private static string ConvertDictionaryToJsonBody(IDictionary<string, object> dict)
        {
            // Converting to System.Collections.Generic.Dictionary<> to ensure Dictionary will be converted to Json in correct format
            var dictSystem = new Dictionary<string, object>(dict);
            var json = JsonConvert.SerializeObject((object)dictSystem, Formatting.None, new RawConverter());
            return json.Substring(1, json.Length - 2);
        }

        private static string ConvertColumnDefsToJson(IEnumerable<ColDef> columns, IEnumerable<CustomButton> buttons)
        {
            Func<bool, bool> isFalse = x => x == false;
            Func<string, bool> isNonEmptyString = x => !string.IsNullOrEmpty(x);

            var defs = new List<JObject>();


            int colIndex = 0;
            columns.Where(t => !string.IsNullOrEmpty(t.Name))
               .ToList().ForEach(t =>
               {

                   JObject colJson = new JObject();
                   JArray targetJson = new JArray(colIndex++);
                   colJson["sName"] = t.Name;
                   colJson["aTargets"] = targetJson;
                   defs.Add(colJson);
               });


            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "bSortable",
                propertySelector: column => column.Sortable,
                propertyPredicate: isFalse,
                columns: columns));
            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "bVisible",
                propertySelector: column => column.Visible,
                propertyPredicate: isFalse,
                columns: columns));
            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "bSearchable",
                propertySelector: column => column.Searchable,
                propertyPredicate: isFalse,
                columns: columns));
            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "mRender",
                propertySelector: column => column.MRenderFunction,
                propertyConverter: x => new JRaw(x),
                propertyPredicate: isNonEmptyString,
                columns: columns));
            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "className",
                propertySelector: column => column.CssClass,
                propertyPredicate: isNonEmptyString,
                columns: columns));
            defs.AddRange(ConvertColumnDefsToTargetedProperty(
                jsonPropertyName: "width",
                propertySelector: column => column.Width,
                propertyPredicate: isNonEmptyString,
                columns: columns));

            int customTarget = 0;
            //Append Custom button columns starting in the order View/Edit/Delete/Custom buttons.
            //The target will start from -1 (right side) and hence the custom buttons have to be added first.
            //the View button will be added at the end

            ////buttons.Where(t => t.Functionality == CustomButtonFunction.Custom)
            ////    .ToList().ForEach(t =>
            ////    {
            ////        defs.Add(ConvertColumnDefsToButton(t, ++customTarget));
            ////    });
            ////var customButton = buttons.Where(t => t.Functionality == CustomButtonFunction.Delete).FirstOrDefault();
            ////if (customButton != null)
            ////{
            ////    defs.Add(ConvertColumnDefsToButton(customButton, ++customTarget));
            ////}

            ////customButton = buttons.Where(t => t.Functionality == CustomButtonFunction.Edit).FirstOrDefault();
            ////if (customButton != null)
            ////{
            ////    defs.Add(ConvertColumnDefsToButton(customButton, ++customTarget));
            ////}

            ////customButton = buttons.Where(t => t.Functionality == CustomButtonFunction.View).FirstOrDefault();
            ////if (customButton != null)
            ////{
            ////    defs.Add(ConvertColumnDefsToButton(customButton, ++customTarget));
            ////}

            var defsbuttons = new List<JObject>();

            for (int ind = buttons.Count() - 1; ind >= 0; ind--)
            {
                if (buttons.ElementAt(ind).Functionality == CustomButtonFunction.Add || buttons.ElementAt(ind).Functionality == CustomButtonFunction.None)
                {
                    continue;
                }

                defsbuttons.Add(ConvertColumnDefsToButton(buttons.ElementAt(ind), ++customTarget));
            }



            defs.AddRange(defsbuttons);


            if (defs.Count > 0)
                return JsonConvert.SerializeObject(defs);

            return "[]";
        }



        private static string ConvertColumnsToJson(IEnumerable<ColDef> columns)
        {
            Func<bool, bool> isFalse = x => x == false;
            Func<string, bool> isNonEmptyString = x => !string.IsNullOrEmpty(x);

            var defs = new List<dynamic>();
            columns//.Where(t => !string.IsNullOrEmpty(t.Name))
                .ToList().ForEach(t =>
                {

                    JObject colJson = new JObject();
                    colJson["data"] = t.Name;
                    defs.Add(colJson);
                });
            if (defs.Count > 0)
                return JsonConvert.SerializeObject(defs);

            return "[]";
        }

        private static string ConvertColumnSortingToJson(IEnumerable<ColDef> columns)
        {
            var sortList = columns.Select((c, idx) => c.SortDirection == SortDirection.None ? new dynamic[] { -1, "" } : (c.SortDirection == SortDirection.Ascending ? new dynamic[] { idx, "asc" } : new dynamic[] { idx, "desc" })).Where(x => x[0] > -1).ToArray();

            if (sortList.Length > 0)
                return JsonConvert.SerializeObject(sortList);

            return "[]";
        }

        private static IDictionary<string, object> ConvertObjectToDictionary(object obj)
        {
            var d = new Dictionary<string, object>();
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                d[propertyInfo.Name] = propertyInfo.GetValue(obj);
            }
            return d;
        }

        private static IEnumerable<JObject> ConvertColumnDefsToTargetedProperty<TProperty>(
            string jsonPropertyName,
            Func<ColDef, TProperty> propertySelector,
            Func<TProperty, bool> propertyPredicate,
            IEnumerable<ColDef> columns)
        {
            return ConvertColumnDefsToTargetedProperty(
                jsonPropertyName,
                propertySelector,
                propertyPredicate,
                x => x,
                columns);
        }


        private static JObject ConvertColumnDefsToButton(
            CustomButton button, int target)
        {
            JObject buttonJson = new JObject();
            buttonJson["aTargets"] = target * -1;
            buttonJson["data"] = null;
            string text = !string.IsNullOrEmpty(button.LabelText) ? button.LabelText :
                string.Format("<span class=\"{0}\"></span>", button.Glyphicon);
            string clickEvent = string.IsNullOrEmpty(button.GetClickEvent()) ?
                string.Empty :
                button.GetClickEvent();
            string callbackEvent =
                string.IsNullOrEmpty(button.CallBackEvent) ? string.Empty :
                string.Format("{0}({1}());", button.CallBackEvent, button.GetEventForDataArg());
            buttonJson["defaultContent"] = string.Format("<button class='center-block' onclick=\"{0};{1};\">{2}</button>", clickEvent, callbackEvent, text);

            return buttonJson;
        }


        private static IEnumerable<JObject> ConvertColumnDefsToTargetedProperty<TProperty, TResult>(
            string jsonPropertyName,
            Func<ColDef, TProperty> propertySelector,
            Func<TProperty, bool> propertyPredicate,
            Func<TProperty, TResult> propertyConverter,
            IEnumerable<ColDef> columns)
        {
            return columns
                .Select((x, idx) => new { rawPropertyValue = propertySelector(x), idx })
                .Where(x => propertyPredicate(x.rawPropertyValue))
                .GroupBy(
                    x => x.rawPropertyValue,
                    (rawPropertyValue, groupedItems) => new
                    {
                        rawPropertyValue,
                        indices = groupedItems.Select(x => x.idx)
                    })
                .Select(x => new JObject(
                    new JProperty(jsonPropertyName, propertyConverter(x.rawPropertyValue)),
                    new JProperty("aTargets", new JArray(x.indices))
                    ));
        }


        public void AddButtoncolumn(CustomButton button)
        {

            switch (button.Functionality)
            {
                case CustomButtonFunction.None:
                    throw new Exception("Please set the button Functionality");
                case CustomButtonFunction.View:
                    button.Glyphicon = "glyphicon glyphicon-eye-open";
                    button.HeaderText = string.IsNullOrEmpty(button.HeaderText) ? "View" : button.HeaderText;
                    button.SetClickEvent(string.Format("{0}()", this.ViewClick));
                    break;
                case CustomButtonFunction.Add:
                    button.Glyphicon = "glyphicon glyphicon-plus";
                    button.HeaderText = string.IsNullOrEmpty(button.HeaderText) ? "Add" : button.HeaderText;
                    //button.SetClickEvent(this.AddClick);
                    button.SetClickEvent(string.Format("{0}({1})", this.EditClick, (int)CustomButtonFunction.Add));
                    break;
                case CustomButtonFunction.Edit:
                    button.Glyphicon = "glyphicon glyphicon-edit";
                    button.HeaderText = string.IsNullOrEmpty(button.HeaderText) ? "Edit" : button.HeaderText;
                    button.SetClickEvent(string.Format("{0}({1})", this.EditClick, (int)CustomButtonFunction.Edit));
                    break;
                case CustomButtonFunction.Delete:
                    button.Glyphicon = "glyphicon glyphicon-remove";
                    button.HeaderText = string.IsNullOrEmpty(button.HeaderText) ? "Delete" : button.HeaderText;
                    button.SetClickEvent(string.Format("{0}()", this.DeleteClick));
                    break;
                case CustomButtonFunction.Custom:
                    break;
            }


            switch (button.Functionality)
            {
                case CustomButtonFunction.Add:
                case CustomButtonFunction.Edit:
                case CustomButtonFunction.Delete:
                    if (string.IsNullOrEmpty(this.AjaxPostUrl))
                    {
                        throw new Exception("Please set the AjaxPostUrl for Add/Edit/Delete row functionality");
                    }
                    //Check that this is the only one
                    if (this.CustomButtons.Where(tt => tt.Functionality == button.Functionality).Count() > 0)
                    {
                        throw new Exception("Default button function already added. Cannot be duplicated");
                    }

                    break;
                case CustomButtonFunction.Custom:
                    if (string.IsNullOrEmpty(button.CallBackEvent))
                    {
                        throw new Exception("Please set the callback Event to add custom button");
                    }
                    break;
            }

            button.SetEventForDataArg(this.DataArgFetch);

            if (string.IsNullOrEmpty(button.Glyphicon))
            {
                button.LabelText = string.IsNullOrEmpty(button.LabelText) ? button.Functionality.ToString() : button.LabelText;
            }


            //Update the Column and Button list
            switch (button.Functionality)
            {
                case CustomButtonFunction.View:
                case CustomButtonFunction.Edit:
                case CustomButtonFunction.Delete:
                case CustomButtonFunction.Custom:
                    var cols = this.Columns.ToList();
                    cols.Add(new Models.ColDef(button.LabelText, typeof(string))
                    {
                        Visible = true,
                        Sortable = false,
                        DisplayName = button.HeaderText,
                        Editable = false,
                        Name = string.Empty
                    });
                    this.Columns = cols;
                    this.CustomButtons.Add(button);
                    break;
                case CustomButtonFunction.Add:
                    this.CustomButtons.Add(button);
                    break;
            }
        }
    }
}
