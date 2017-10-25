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
    public static class NumericTextBoxExtensions
    {
        /// <summary>
        /// 0 - element Id
        /// 1 - Before Call Function and Data Manipulation to preselected value 
        /// 2 - Date Format
        /// </summary>
        /// scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, selectedValue, optionLblStr, setDisableString);
        private const string Js1CreateInitFunction = @"function initNumTextBoxFor{0}() {{                           
        {{ 
                //Add Change Event
                $('#{0}').on('change', function(ev){{
                        {1};
                }});                                  
            ";



        /// <summary>
        ///     User key will be validated to ensure only integers are allowed including negative     
        /// </summary>       
        private const string Js2CheckIntFormat =
          @"
             $('#{0}').keypress(function (e) {{
                 //if the letter is not digit then display error and don't type anything
                 if (e.which != 8 && e.which != 0 && e.which != 45 && (e.which < 48 || e.which > 57)) {{
                    //display error message
                    $('#{1}').html('Digits Only').show().fadeOut('slow');
                           return false;
                 }}
             }});
            ";

        /// <summary>
        ///     User key will be validated to ensure only decimal are allowed including negative     
        /// </summary>       
        private const string Js2CheckDecimalFormat =
          @"
             $('#{0}').keypress(function (e) {{
                 //if the letter is not digit then display error and don't type anything
                 if (e.which != 8 && e.which != 0 && e.which != 45 && e.which != 46 && (e.which < 48 || e.which > 57)) {{
                    //display error message
                    $('#{1}').html('Digits Only').show().fadeOut('slow');
                           return false;
                 }}
            }});
            ";

        /// <summary>
        /// Last in order 
        /// {0} - cascading dropdown element Id
        /// </summary>
        private const string Js7EndFormat = @"
            }};
        
        }};

        if (document.readyState != 'loading') {{
            initNumTextBoxFor{0}();
        }} else {{
            document.addEventListener('DOMContentLoaded', initNumTextBoxFor{0});
        }}";






        public static MvcHtmlString NumericTextBoxCustom<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            NumericTextInput<TModel> input,
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


            string labelText = input.LabelText;
            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(input.ModelProperty, htmlHelper.ViewData);
            ModelMetadata metadata = ModelMetadata.FromStringExpression(input.ModelPropertyName, htmlHelper.ViewData);
            if (metadata != null && input.ShowLabel && string.IsNullOrEmpty(labelText))
            {
                labelText = metadata.DisplayName ?? metadata.PropertyName.ToTitleCaseFromPascal();
            }


            var control = ControlHelpers.AssignEditControl(typeof(TModel));
            input.Type = input.Type == NumericType.Default ?
                (control == DataTable.Infrastructure.EditControl.Decimal ? NumericType.Decimal : NumericType.Int) : input.Type;


            return BuildControl(
                htmlHelper,
                elementName,
                elementId,
                ControlHelpers.GetPropStringValue(htmlHelper.ViewData.Model, input.ModelProperty),
                input.WidthPerCent,
                input.Type,
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



        public static MvcHtmlString NumericTextBoxCustom(
           this HtmlHelper htmlHelper,
           NumericTextInput<string> input,
           TextCallBackOptions options = null)
        {
            if (input.IdAttr == null)
            {
                throw new ArgumentException("IdAttr is not set");
            }

            if (input.Type == NumericType.Default)
            {
                throw new ArgumentException("Type is not set");
            }

            var elementId = input.IdAttr;
            var elementName = elementId;


            string labelText = input.LabelText;

            if (input.ShowLabel && !string.IsNullOrEmpty(labelText))
            {
                labelText = input.Name.ToTitleCaseFromPascal();
            }

            return BuildControl(
                htmlHelper,
                elementName,
                elementId,
                input.Value,
                input.WidthPerCent,
                input.Type,
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
            int widthPercent,
            NumericType type,
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

            //Add Number type 
            htmlAttributes.Add("type", "number");


            //Add the default class : form-control
            if (htmlAttributes["class"] == null)
            {
                htmlAttributes.Add("class", "form-control");
            }
            else
            {
                htmlAttributes["class"] = "form-control " + htmlAttributes["class"];
            }


            var removeDisabledString = string.Empty;

            if (placeHolder != null)
            {
                htmlAttributes.Add("placeholder", placeHolder);
            }


            var defaultLabelHtml = string.Empty; // htmlHelper.LabelFor()
            if (!string.IsNullOrEmpty(labelText))
            {
                defaultLabelHtml = htmlHelper.LabelFor(labelText, inputId, htmlLabelAttributes).ToHtmlString();
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

            MvcHtmlString defaultControlHtml = new MvcHtmlString(string.Empty);
            MvcHtmlString hiddenControlHtml = new MvcHtmlString(string.Empty);


            switch (access)
            {
                case ControlAccess.Edit:
                    defaultControlHtml = htmlHelper.TextBox(
                inputId,
                selectedValue,
                htmlAttributes);
                    break;
                case ControlAccess.Read:
                    defaultControlHtml = htmlHelper.Label(selectedValue, inputId, htmlAttributes);
                    break;
                case ControlAccess.ReadPost:
                    defaultControlHtml = htmlHelper.Label(selectedValue, inputId, htmlAttributes);
                    hiddenControlHtml = htmlHelper.Hidden(inputName, selectedValue);
                    break;

            }

            string errId = string.Format("{0}Numerr", inputId);
            
            var scriptBuilder = new StringBuilder();
            var onChange = string.Empty;
            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.OnChange))
                {
                    onChange = string.Format("{0}(this.value);", options.OnChange);
                }

            }

            scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, onChange);
            if(type == NumericType.Decimal)
                scriptBuilder.AppendFormat(Js2CheckIntFormat, inputId, errId);
            else
                scriptBuilder.AppendFormat(Js2CheckDecimalFormat, inputId, errId);

            scriptBuilder.AppendFormat(Js7EndFormat, inputId);
            var script = string.Concat("<script>", scriptBuilder.ToString(), "</script>");

            return new MvcHtmlString(
                string.Concat(
                    string.Format(BuildHtmlPlaceHolder(), defaultLabelHtml.ToString(), hiddenControlHtml.ToString(),
                    defaultControlHtml.ToString(), string.Format("<span id='{0}'></span>", errId)),
                    Environment.NewLine, script));
        }


        private static string BuildHtmlPlaceHolder()
        {
            return @"<div class=""form-group"">{0}<br/>{1}{2}{3}</div>";
        }



       


    }
}