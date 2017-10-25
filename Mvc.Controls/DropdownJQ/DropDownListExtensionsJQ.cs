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
    public static class DropDownListExtensionsJQ
    {
        //private const string CssInit = @".overflow{0}{{ height: {1}px; }}";

        /// <summary>
        /// 0 - dropdown element Id
        /// 1 - Before Call Function and Data Manipulation to preselected value        
        /// 2 - if optionLabel is set should be set to '<option value="""">optionLabel</option>' otherwise should be set to ""        
        /// </summary>
        /// scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, selectedValue, optionLblStr, setDisableString);
        private const string Js1CreateInitFunction = @"function initDropDownFor{0}() {{        
        var targetElement = document.getElementById('{0}');
        if(targetElement == null)
            return;

        var preselectedValue = '{1}';        
        {{                     
            var items = {2};
            var ajaxUri = '{4}';
            var edit = '{5}';
            var setHeight = '{6}';
            if (true) {{
                targetElement.innerHTML = items;                
                var event = document.createEvent('HTMLEvents');
                event.initEvent('change', true, false);
                targetElement.dispatchEvent(event);   
                if(edit == 'True'){{
                    $('#{0}').selectmenu({{width: '{3}%'}});
                    //if(setHeight == 'True')
                    //    $('#{0}').selectmenu().selectmenu('menuWidget').css('height', '{7}px');
                        //$('#{0}').selectmenu().selectmenu('menuWidget').addClass('overflow{0}');
                }}
            }}";


        /// <summary>
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName 
        /// {1} - CascadeDropDownOptions.BeforeDataLoad function name        
        /// </summary>
        private const string Js2CallBeforeDataLoadFunctionFormat = @"            
            {1}({{ {0} : value }});            
            ";


        /// <summary>
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName         
        /// {2} - CascadeDropDownOptions.ManipulateDataSend function name
        /// </summary>
        private const string Js2GenerateJsonToSendFromFunctionFormat = @"
            var jsonToSend = {{ {0} }};            
            var updatedJson = {1}(jsonToSend);
            if(updatedJson){{jsonToSend = updatedJson}}
            ";

        /// <summary>
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName
        /// </summary>
        private const string Js2SimpleGenerateJsonToSendFormat = @"var jsonToSend = {{ {0} }};";

        /// <summary>
        /// 3 in order CONDITIONAL
        /// used when CascadeDropDownOptions.HttpMethod is set to POST
        /// </summary>
        private const string Js3InitializePostRequest =
            @"var url = ajaxUri;            
             $.ajax({{ 
                type: 'POST', 
                url: url, 
                data: jsonToSend, 
                cache : false,
                dataType: 'json',
                success: success{0},
                error: error{0},
                complete : complete{0}
            }});";

        /// <summary>
        /// 3 in order CONDITIONAL
        /// used when CascadeDropDownOptions.HttpMethod is not set, or set to GET.
        /// </summary>
        private const string Js3InitializeGetRequest =
          @"var url = ajaxUri;var appndSgn = url.indexOf('?') > -1 ? '&' : '?';
            var qs = Object.keys(jsonToSend).map(function(key){{return key+'='+jsonToSend[key]}}).join('&');
            $.ajax({{ 
                type: 'GET', 
                url: url + appndSgn + qs, 
                data: {{ get_param: 'value' }}, 
                cache : false,
                dataType: 'json',
                success: success{0},
                error: error{0},
                complete : complete{0}
            }});";

        /// <summary>
        /// 4 in order
        /// {0} -  will have a call to CascadeDropDownOptions.OnCompleteGetData if it was set.
        /// {1} -  will have a call to CascadeDropDownOptions.OnSuccessGetData if it was set.
        /// {2} -  will have a call to CascadeDropDownOptions.OnFailureGetData if it was set.
        /// {3} -  Append the name of the Control
        /// {4} -  On Change Event
        /// </summary>       
        private const string Js4OnLoadFormat =
          @"var isSelected = false;
            function success{3}(data, status, xhr) {{  
                {1}
                if (data) {{
                    
                   //set the height of the dropdown
                    var space = window.innerHeight - $('#{3}').closest('.form-group').offset().top;
                    if(data.length * 30 > space)
                    {{        
                        var heightManip = space > 300 ? (space * 0.85) : 300;                
                        $('#{3}').selectmenu().selectmenu('menuWidget').css('height', heightManip);

                    }}

                    data.forEach(function(item, i) {{ 
                        if(edit == 'True')
                        {{
                            items += '<option value=""' + item.Value + '""'
                            if(item.Disabled){{items += ' disabled'}}
                            items += '>' + item.Text + '</option>';
                        }}
                        else
                        {{
                            if(item.Value == preselectedValue)
                            {{
                                targetElement.innerHTML = item.Text;
                                targetElement.value = item.Value;
                            }}
                        }}
                    }});
                    if(edit == 'True')
                    {{
                        targetElement.innerHTML = items;
                    }}
                    if(preselectedValue && edit == 'True')
                    {{ 

                        $('#{3}').val(preselectedValue);                         
                        $('#{3}').selectmenu('refresh');
                        $('#{3}').trigger('selectmenuchange');   
                        preselectedValue = null;
                    }}
               
                    //var event = document.createEvent('HTMLEvents');
                    //event.initEvent('change', true, false);
                    //targetElement.dispatchEvent(event);
                     if(edit == 'True')
                     {{
                        $('#{3}').on('selectmenuchange', function() {{
                            {4};
                        }});   
                    }}
                    else
                    {{
                        var event = document.createEvent('HTMLEvents');
                        event.initEvent('change', true, false);
                        targetElement.dispatchEvent(event);                            
                        targetElement.addEventListener('change', function(){{
                                {4};
                        }});
                    }}                    

                }}
            }};
            
            function complete{3}(xhr, status) {{
                {0}
            }};

            function error{3}(jqXHR, status, exception) {{
                var msg = '';
                if (jqXHR.status === 0) {{
                    msg = 'Not connect.\n Verify Network.';
                }} else if (jqXHR.status == 404) {{
                    msg = 'Requested page not found. [404]';
                }} else if (jqXHR.status == 500) {{
                    msg = 'Internal Server Error [500].';
                }} else if (exception === 'parsererror') {{
                    msg = 'Requested JSON parse failed.';
                }} else if (exception === 'timeout') {{
                    msg = 'Time out error.';
                }} else if (exception === 'abort') {{
                    msg = 'Ajax request aborted.';
                }} else {{
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }}
                    alert(msg, 'error');
                {2}  
            }};";



        /// <summary>
        /// Last in order 
        /// {0} - cascading dropdown element Id
        /// </summary>
        private const string Js7EndFormat = @"
        }};
        
    }};

    if (document.readyState != 'loading') {{
        initDropDownFor{0}();
    }} else {{
        document.addEventListener('DOMContentLoaded', initDropDownFor{0});
    }}";



        public static MvcHtmlString DropDownListJQCustom<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            DropdownInput<TModel> input,
            SourceCallBackOptions options = null)
        {


            if (input.ModelProperty == null)
            {
                throw new ArgumentException("ModelProperty is not set");
            }


            var dropDownElementName = string.IsNullOrEmpty(input.Name) ? htmlHelper.GetElementNameFromExpression(input.ModelProperty) : input.Name;
            var dropDownElementId = string.IsNullOrEmpty(input.IdAttr) ? (ControlHelpers.GetElementIdFromHtmlAttributes(input.DataHtmlAttributes) ??
                htmlHelper.GetElementIdFromExpression(input.ModelProperty)) : input.IdAttr;


            if (string.IsNullOrEmpty(dropDownElementName) || string.IsNullOrEmpty(dropDownElementId))
            {

                dropDownElementName = htmlHelper.GetElementNameFromExpression(input.ModelPropertyName);
                dropDownElementId = ControlHelpers.GetElementIdFromHtmlAttributes(input.DataHtmlAttributes) ??
                    htmlHelper.GetElementIdFromExpression(input.ModelPropertyName);
                if (string.IsNullOrEmpty(dropDownElementName) || string.IsNullOrEmpty(dropDownElementId))
                {
                    throw new ArgumentException("expression argument is invalid");
                }
            }


            string labelText = input.LabelText;
            if (string.IsNullOrEmpty(labelText))
            {
                ModelMetadata metadata = ModelMetadata.FromStringExpression(input.ModelPropertyName, htmlHelper.ViewData);
                if (metadata != null && input.ShowLabel)
                {
                    labelText = metadata.DisplayName ?? metadata.PropertyName.ToTitleCaseFromPascal();
                }
            }

            return BuildControl(
                htmlHelper,
                dropDownElementName,
                dropDownElementId,
                input.DataUri,
                input.DataUriParamName,
                input.DataUriParamValue,
                ControlHelpers.GetPropStringValue(htmlHelper.ViewData.Model, input.ModelProperty),
                input.WidthPerCent,
                input.HeightPixel,
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


        public static MvcHtmlString DropDownListJQCustom(
            this HtmlHelper htmlHelper,
            DropdownInput<string> input,
            SourceCallBackOptions options = null)
        {
            if (input.IdAttr == null)
            {
                throw new ArgumentException("ModelProperty is not set");
            }
            var dropDownElementName = input.IdAttr;
            var dropDownElementId = input.IdAttr;




            string labelText = input.LabelText;
            if (string.IsNullOrEmpty(labelText))
            {

                labelText = input.Name.ToTitleCaseFromPascal();

            }

            return BuildControl(
                htmlHelper,
                dropDownElementName,
                dropDownElementId,
                input.DataUri,
                input.DataUriParamName,
                input.DataUriParamValue,
                input.Value,
                input.WidthPerCent,
                input.HeightPixel,
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
            string ajaxUri,
            string ajaxActionParamName,
            string ajaxActionParamValue,
            string selectedValue,
            int widthPercent,
            int heightPixel,
            ControlAccess access = ControlAccess.Edit,
            string optionLabel = null,
            RouteValueDictionary htmlAttributes = null,
            string labelText = null,
            RouteValueDictionary htmlLabelAttributes = null,
            SourceCallBackOptions options = null)
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

            htmlAttributes.Add("data-cascade-dd-url", ajaxUri);

            var removeDisabledString = string.Empty;

            if (optionLabel != null)
            {
                htmlAttributes.Add("data-option-lbl", optionLabel);
            }


            var defaultLabelHtml = string.Empty; // htmlHelper.LabelFor()
            if (!string.IsNullOrEmpty(labelText))
            {
                defaultLabelHtml = htmlHelper.LabelFor(labelText, inputName, htmlLabelAttributes).ToHtmlString();
            }
            MvcHtmlString defaultControlHtml = new MvcHtmlString(string.Empty);
            MvcHtmlString hiddenControlHtml = new MvcHtmlString(string.Empty);
            switch (access)
            {
                case ControlAccess.Edit:
                    defaultControlHtml = htmlHelper.DropDownList(
                inputName,
                new List<SelectListItem>(),
                optionLabel,
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
            var optionLblStr = optionLabel == null ? "''" : string.Format(@"'<option value="""">{0}</option>'", optionLabel);
            scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, selectedValue,
                optionLblStr, widthPercent,
                ajaxUri, access == ControlAccess.Edit, heightPixel > 0, heightPixel);
            //scriptBuilder.AppendFormat(Js1CreateInitFunction, inputId, triggeredByProperty, selectedValue, removeDisabledString, optionLblStr, setDisableString);
            //ApplyCallBeforeDataLoadFunctionString(ref scriptBuilder, ajaxActionParamName, options);
            ApplyJsonToSendString(ref scriptBuilder, ajaxActionParamName, ajaxActionParamValue, options);
            ApplyRequestString(ref scriptBuilder, inputId, options);
            ApplyOnLoadString(ref scriptBuilder, inputId, options);
            //ApplyErrorCallbackString(ref scriptBuilder, options);
            //ApplySendRequestString(ref scriptBuilder, options);
            scriptBuilder.AppendFormat(Js7EndFormat, inputId);

            var cssBuilder = new StringBuilder();
            //cssBuilder.AppendFormat(CssInit, inputId, heightPixel);
            var script = string.Concat("<script>", scriptBuilder.ToString(), "</script>");
            var css = string.Concat("<style>", cssBuilder.ToString(), "</style>");
            return new MvcHtmlString(
                string.Concat(
                    string.Format(BuildHtmlPlaceHolder(), defaultLabelHtml.ToString(), defaultControlHtml.ToString(),
                    hiddenControlHtml.ToString()),
                    Environment.NewLine, css, script));
        }


        private static string BuildHtmlPlaceHolder()
        {
            return @"<div class=""form-group"">{0}<br/>{1}{2}</div>";
        }



        private static void ApplyJsonToSendString(ref StringBuilder builder, string ajaxParam, string ajaxValue, SourceCallBackOptions options)
        {
            string dataToSend = !string.IsNullOrEmpty(ajaxParam) ?
                string.Format("{0} : '{1}'", ajaxParam, ajaxValue) : string.Empty;

            builder.Append(options == null || string.IsNullOrEmpty(options.ManipulateDataSend) ?
                string.Format(Js2SimpleGenerateJsonToSendFormat, dataToSend) :
                string.Format(Js2GenerateJsonToSendFromFunctionFormat, dataToSend, options.ManipulateDataSend));
        }

        private static void ApplyRequestString(ref StringBuilder builder, string inputId, SourceCallBackOptions options)
        {
            builder.Append(options == null || options.HttpMethod != HttpVerb.POST ?
                string.Format(Js3InitializeGetRequest, inputId) :
                string.Format(Js3InitializePostRequest, inputId));
        }

        private static void ApplyOnLoadString(ref StringBuilder builder, string inputId, SourceCallBackOptions options)
        {
            var onComplete = string.Empty;
            var onSuccess = string.Empty;
            var onFailure = string.Empty;
            var onChange = string.Empty;
            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.OnCompleteGetData))
                {
                    onComplete = string.Format("{0}(status);", options.OnCompleteGetData);
                }
                if (!string.IsNullOrEmpty(options.OnSuccessGetData))
                {
                    onSuccess = string.Format("{0}(data);", options.OnSuccessGetData);
                }
                if (!string.IsNullOrEmpty(options.OnFailureGetData))
                {
                    onFailure = string.Format("{0}(msg, jqXHR.status, jqXHR.statusText);", options.OnFailureGetData);
                }
                if (!string.IsNullOrEmpty(options.OnChange))
                {
                    onChange = string.Format("{0}();", options.OnChange);
                }
            }
            builder.AppendFormat(Js4OnLoadFormat, onComplete, onSuccess, onFailure, inputId, onChange);
        }



    }
}