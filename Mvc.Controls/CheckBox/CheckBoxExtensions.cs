using Mvc.Common;
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
    public static class CheckBoxExtensions
    {


        /// <summary>
        /// 0 - element Id
        /// 1 - Before Call Function and Data Manipulation to preselected value 
        /// 2 - Date Format
        /// </summary>
        /// scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, selectedValue, optionLblStr, setDisableString);
        private const string Js1CreateInitFunction = @"function initCheckBoxFor{0}() {{                           
        {{ 
                //Add Change Event
                    $('#{0}').on('change', function(ev){{
                        {1};
                }});                                  
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
            initCheckBoxFor{0}();
        }} else {{
            document.addEventListener('DOMContentLoaded', initCheckBoxFor{0});
        }}";






        public static MvcHtmlString CheckBoxCustom<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            CheckBoxInput<TModel> input,
            CheckCallBackOptions options = null)
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

            var valueModal = ControlHelpers.GetPropStringValue(htmlHelper.ViewData.Model, input.ModelProperty);

            bool value = Helper.ParseBool(valueModal);           
            
            return BuildControl(
                htmlHelper,
                elementName,
                elementId,
                value,
                input.DefaultCheckedValue,
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



        public static MvcHtmlString CheckBoxCustom(
           this HtmlHelper htmlHelper,
           CheckBoxInput<bool> input,
           CheckCallBackOptions options = null)
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

            return BuildControl(
               htmlHelper,
               elementName,
               elementId,
               input.Value,
               input.DefaultCheckedValue,
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
            bool value,
            bool checkedValueDefault,
            int widthPercent,
            ControlAccess access = ControlAccess.Edit,
            string placeHolder = null,
            RouteValueDictionary htmlAttributes = null,
            string labelText = null,
            RouteValueDictionary htmlLabelAttributes = null,
            CheckCallBackOptions options = null)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new RouteValueDictionary();
            }

            //Add the default class : form-control
            if (htmlAttributes["class"] == null)
            {
                htmlAttributes.Add("class", "");
            }
            else
            {
                htmlAttributes["class"] = " " + htmlAttributes["class"];
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
                //htmlAttributes.Add("style", string.Format("width:{0}% !important;", widthPercent));
            }
          

            MvcHtmlString defaultControlHtml = new MvcHtmlString(string.Empty);
            MvcHtmlString hiddenControlHtml = new MvcHtmlString(string.Empty);

            switch (access)
            {
                case ControlAccess.Edit:
                    defaultControlHtml = htmlHelper.CheckBox(
                inputId,
                value == checkedValueDefault,
                htmlAttributes);
                    break;
                case ControlAccess.Read:                    
                    if (htmlAttributes["disabled"] == null)
                    {
                        htmlAttributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        htmlAttributes["disabled"] = "disabled";
                    }
                    defaultControlHtml = htmlHelper.CheckBox(
                 inputId,
                 value == checkedValueDefault,
                 htmlAttributes);
                    break;
                case ControlAccess.ReadPost:
                    if (htmlAttributes["disabled"] == null)
                    {
                        htmlAttributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        htmlAttributes["disabled"] = "disabled";
                    }
                    defaultControlHtml = htmlHelper.CheckBox(
               inputId,
               value == checkedValueDefault,
               htmlAttributes);
                    hiddenControlHtml = htmlHelper.Hidden(inputName, value == checkedValueDefault);
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

            scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, onChange);
            ApplyOnLoadString(ref scriptBuilder, inputId, options);
            scriptBuilder.AppendFormat(Js7EndFormat, inputId);
            var script = string.Concat("<script>", scriptBuilder.ToString(), "</script>");

            return new MvcHtmlString(
                string.Concat(
                    string.Format(BuildHtmlPlaceHolder(), defaultLabelHtml.ToString(), hiddenControlHtml.ToString(),
                    defaultControlHtml.ToString()),
                    Environment.NewLine, script));
        }


        private static string BuildHtmlPlaceHolder()
        {
            return @"<div class=""form-group"">{0}<br/>{1}{2}</div>";
        }



        private static void ApplyOnLoadString(ref StringBuilder builder, string inputId, CheckCallBackOptions options)
        {

            builder.AppendFormat(Js4OnLoadFormat);
        }


    }
}