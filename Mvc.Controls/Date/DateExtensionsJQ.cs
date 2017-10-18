using Mvc.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Mvc.Controls
{
    public static class DateExtensionsJQ
    {


        /// <summary>
        /// 0 - element Id
        /// 1 - Before Call Function and Data Manipulation to preselected value 
        /// 2 - Date Format
        /// </summary>
        /// scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, selectedValue, optionLblStr, setDisableString);
        private const string Js1CreateInitFunction = @"function initDateFor{0}() {{        
        var targetElement = document.getElementById('{0}');
        var hide = '{4}';
        var preselectedValue = '{1}';        
        {{
            if(hide == 'True')
                return;
            var ctrl = $('#{0}').data('datepicker');
            if(ctrl == undefined)
            {{
                //Add Change Event
                    $('#{0}').on('change', function(ev){{
                        {3};
                }});

                $('#{0}').datepicker({{
                                    dateFormat: '{2}',
                                    changeYear: true,
                                    showOn: 'button',
                                    ////onSelect: function(dateText) {{
                                    ////         {3};
                                    ////}}
                                }}).css('display', 'inline-block')
                                .next('button').button({{
                    icons: {{ primary: 'ui-icon-calendar' }},
                                    label: 'Select Date',
                                    text: false
                                }});  

                   
 
            }}                          
            ";


        /// <summary>
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName 
        /// {1} - CascadeDropDownOptions.BeforeDataLoad function name        
        /// </summary>
        private const string Js2CallBeforeDataLoadFunctionFormat = @"            
            {1}({{ {0} : value }});            
            ";







        /// <summary>
        /// 4 in order
        /// {0} -  will have a call to CascadeDropDownOptions.OnCompleteGetData if it was set.
        /// {1} -  will have a call to CascadeDropDownOptions.OnSuccessGetData if it was set.
        /// {2} -  will have a call to CascadeDropDownOptions.OnFailureGetData if it was set.
        /// {3} -  Append the name of the Control
        /// </summary>       
        private const string Js4OnLoadFormat =
          @"";



        /// <summary>
        /// Last in order 
        /// {0} - cascading dropdown element Id
        /// </summary>
        private const string Js7EndFormat = @"
            }};
        
        }};

        if (document.readyState != 'loading') {{
            initDateFor{0}();
        }} else {{
            document.addEventListener('DOMContentLoaded', initDateFor{0});
        }}";






        public static MvcHtmlString DateCustom<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            DateInput<TModel> input,
            TextCallBackOptions options = null)
        {
            if (input.ModelProperty == null)
            {
                throw new ArgumentException("ModelProperty is not set");
            }


            var elementName = htmlHelper.GetElementNameFromExpression(input.ModelProperty);
            var elementId = ControlHelpers.GetElementIdFromHtmlAttributes(input.DataHtmlAttributes) ??
                htmlHelper.GetElementIdFromExpression(input.ModelProperty);

            if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(elementId))
            {

                elementName = htmlHelper.GetElementNameFromExpression(input.ModelPropertyName);
                elementId = ControlHelpers.GetElementIdFromHtmlAttributes(input.DataHtmlAttributes) ??
                    htmlHelper.GetElementIdFromExpression(input.ModelPropertyName);
                if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(elementId))
                {
                    throw new ArgumentException("expression argument is invalid");
                }
            }


            string labelText = string.Empty;
            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(input.ModelProperty, htmlHelper.ViewData);
            ModelMetadata metadata = ModelMetadata.FromStringExpression(input.ModelPropertyName, htmlHelper.ViewData);
            if (metadata != null && input.ShowLabel)
            {
                labelText = metadata.DisplayName ?? metadata.PropertyName.ToTitleCaseFromPascal();
            }

            return BuildControl(
                htmlHelper,
                elementName,
                elementId,
                ControlHelpers.GetPropStringValue(htmlHelper.ViewData.Model, input.ModelProperty),
                input.DateFormat,
                input.DateFormatJS,
                input.WidthPerCent,
                input.Access,
                input.PlaceHolder,
                input.DataHtmlAttributes != null
                ? HtmlHelper.AnonymousObjectToHtmlAttributes(input.DataHtmlAttributes)
                : new RouteValueDictionary(),
                  labelText,
                input.LabelHtmlAttributes != null
                ? HtmlHelper.AnonymousObjectToHtmlAttributes(input.LabelHtmlAttributes)
                : new RouteValueDictionary(),
                options);
        }

        public static MvcHtmlString DateCustom(
            this HtmlHelper htmlHelper,
            DateInput<DateTime?> input,
            TextCallBackOptions options = null)
        {
            if (input.IdAttr == null)
            {
                throw new ArgumentException("IdAttr is not set");
            }


            var elementId = input.IdAttr;
            var elementName = elementId;


            string labelText = input.LabelText;

            if (input.ShowLabel && !string.IsNullOrEmpty(labelText))
            {
                labelText = input.Name.ToTitleCaseFromPascal();
            }

            string value = input.Value.HasValue ? input.Value.Value.ToString(input.DateFormat) : string.Empty;

            return BuildControl(
                htmlHelper,
                elementName,
                elementId,
                value,
                input.DateFormat,
                input.DateFormatJS,
                input.WidthPerCent,
                input.Access,
                input.PlaceHolder,
                input.DataHtmlAttributes != null
                ? HtmlHelper.AnonymousObjectToHtmlAttributes(input.DataHtmlAttributes)
                : new RouteValueDictionary(),
                  labelText,
                input.LabelHtmlAttributes != null
                ? HtmlHelper.AnonymousObjectToHtmlAttributes(input.LabelHtmlAttributes)
                : new RouteValueDictionary(),
                options);
        }

        private static MvcHtmlString BuildControl(
            this HtmlHelper htmlHelper,
            string inputName,
            string inputId,
            string selectedValue,
            string format,
            string formatJs,
             int widthPercent,
              ControlAccess access = ControlAccess.Edit,
            string placeHolder = null,
            RouteValueDictionary htmlAttributes = null,
            string labelText = null,
            RouteValueDictionary htmlLabelAttributes = null,
            TextCallBackOptions options = null)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new RouteValueDictionary();
            }

            //Add the default class : form-control
            if (htmlAttributes["class"] == null)
            {
                htmlAttributes.Add("class", "form-control");
            }
            else
            {
                htmlAttributes["class"] = "form-control " + htmlAttributes["class"];
            }


            //Add the default class : form-control
            if (htmlAttributes["style"] == null)
            {
                htmlAttributes.Add("style", string.Format("width:{0}% !important;", widthPercent));
            }
            else
            {
                htmlAttributes["style"] = string.Format("width:{0}% !important; ", widthPercent) + htmlAttributes["style"];
            }


            var removeDisabledString = string.Empty;

            if (placeHolder != null)
            {
                htmlAttributes.Add("placeholder", placeHolder);
            }


            var defaultLabelHtml = string.Empty; // htmlHelper.LabelFor()
            if (!string.IsNullOrEmpty(labelText))
            {
                defaultLabelHtml = htmlHelper.LabelFor(labelText, inputName, htmlLabelAttributes).ToHtmlString();
            }
            DateTime parseDate;
            if (DateTime.TryParse(selectedValue, out parseDate))
            {
                selectedValue = parseDate.ToString(format);
            }

            MvcHtmlString defaultControlHtml = new MvcHtmlString(string.Empty);
            MvcHtmlString hiddenControlHtml = new MvcHtmlString(string.Empty);
            switch (access)
            {
                case ControlAccess.Edit:
                    defaultControlHtml = htmlHelper.TextBox(
                inputName,
                selectedValue,
                htmlAttributes);
                    break;
                case ControlAccess.Read:
                    defaultControlHtml = htmlHelper.Label(selectedValue, inputName, htmlAttributes);
                    break;
                case ControlAccess.ReadPost:
                    defaultControlHtml = htmlHelper.Label(selectedValue, inputName, htmlAttributes);
                    hiddenControlHtml = htmlHelper.Hidden(inputName, selectedValue);
                    break;
            }


            var scriptBuilder = new StringBuilder();


            var onChange = string.Empty;
            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.OnChange))
                {
                    onChange = string.Format("{0}(this.value);", options.OnChange);
                }

            }

            scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, 
                selectedValue, formatJs, onChange, access == ControlAccess.Hide);


            ApplyOnLoadString(ref scriptBuilder, inputId, options);
            //ApplyErrorCallbackString(ref scriptBuilder, options);
            //ApplySendRequestString(ref scriptBuilder, options);
            scriptBuilder.AppendFormat(Js7EndFormat, inputId);
            var script = string.Concat("<script>", scriptBuilder.ToString(), "</script>");

            return new MvcHtmlString(
                string.Concat(
                    string.Format(BuildHtmlPlaceHolder(), defaultLabelHtml.ToString(), defaultControlHtml.ToString(),
                    hiddenControlHtml.ToString()),
                    Environment.NewLine, script));
        }


        private static string BuildHtmlPlaceHolder()
        {
            return @"<div class=""form-group"">{0}<br/>{1}{2}</div>";
        }






        private static void ApplyOnLoadString(ref StringBuilder builder, string inputId, TextCallBackOptions options)
        {

            builder.AppendFormat(Js4OnLoadFormat);
        }

   

    }
}