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
    public static class CascadingDropDownListExtensionsJQ
    {
        /// <summary>
        /// 0 - cascading dropdown element Id
        /// 1 - triggeredByProperty - id of parent element that triggers data loading
        /// 2 - Before Call Function and Data Manipulation to preselected value
        /// 3 - if element was initially disabled, will contain removeAttribute('disabled') command
        /// 4 - if optionLabel is set should be set to '<option value="""">optionLabel</option>' otherwise should be set to ""
        /// 5 - if element should be disabled when parent not selected, will contain setAttribute('disabled','disabled') command
        /// </summary>
        private const string Js1CreateInitFunction = @"function initCascadeDropDownFor{0}() {{
        var triggerElement = document.getElementById('{1}');
        var targetElement = document.getElementById('{0}');
        if(targetElement == null || triggerElement == null)
            return;
        var preselectedValue = '{2}';
        var ajaxUri = '{7}';
        var edit = '{8}';
        
        if(edit == 'True')
            $('#{0}').selectmenu({{width: '{6}%'}});


        var trigCtrl = $('#{1}').selectmenu('instance');

        if(trigCtrl != undefined)
        {{
            $('#{1}').on('selectmenuchange', function() {{
                initCascadeDropDownTrigger{0}();
            }});
        }}         
        else
        {{
            console.log('Parent Cascading controls is not JQuery dropdown');    
            triggerElement.onchange = function(e) {{
                        initCascadeDropDownTrigger{0}();
                        }};
        }}
         
        function initCascadeDropDownTrigger{0}() {{
                    if(edit == 'True')
                        $('#{0}').selectmenu('option', 'disabled', false );
                    var value = $('#{1}').val();
                    var items = {4};
                    if (!value) {{
                        targetElement.innerHTML = items;
                        targetElement.value = '';
                        if(edit == 'True')
                        {{
                            $('#{0}').selectmenu('option', 'disabled', true );
                            $('#{0}').selectmenu('refresh');
                        }}
                        return;
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
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName         
        /// {2} - CascadeDropDownOptions.ManipulateDataSend function name
        /// </summary>
        private const string Js2GenerateJsonToSendFromFunctionFormat = @"
            var jsonToSend = {{ {0} : value }};            
            var updatedJson = {1}(jsonToSend);
            if(updatedJson){{jsonToSend = updatedJson}}
            ";

        /// <summary>
        /// 2 in order CONDITIONAL
        /// {0} - ajaxActionParamName
        /// </summary>
        private const string Js2SimpleGenerateJsonToSendFormat = @"var jsonToSend = {{ {0} : value }};";

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
        /// </summary>       
        private const string Js4OnLoadFormat =
          @"var isSelected = false;
            function success{3}(data, status, xhr) {{  
                {1}
                if (data) {{
                    data.forEach(function(item, i) {{
                        if(edit == 'True')
                        {{
                            items += '<option value=""' + item.Value + '""'
                            if(item.Disabled){{items += ' disabled'}}
                            if(item.Value == preselectedValue){{items += selected='selected'}}
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
                    else if(edit == 'True')
                    {{
                        $('#{3}').selectmenu('refresh');
                        $('#{3}').trigger('selectmenuchange');   
                    }}

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
        if(triggerElement.value && !targetElement.value)
        {{
            var event = document.createEvent('HTMLEvents');
            event.initEvent('change', true, false);
            triggerElement.dispatchEvent(event);           
        }} 
    }};

    if (document.readyState != 'loading') {{
        initCascadeDropDownFor{0}();
    }} else {{
        document.addEventListener('DOMContentLoaded', initCascadeDropDownFor{0});
    }}";



        public static MvcHtmlString CascadingDropDownListJQ<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            CascadingInput<TModel> input,
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

            var triggeredByPropId = string.IsNullOrEmpty(input.TriggeredById) ? 
                htmlHelper.GetElementIdFromExpression(input.TriggeredByModelProperty) : input.TriggeredById;

            if (string.IsNullOrEmpty(triggeredByPropId))
            {
                throw new ArgumentException("triggeredByProperty argument is invalid");
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
                triggeredByPropId,
                input.DataUri,
                input.DataUriParamName,
                ControlHelpers.GetPropStringValue(htmlHelper.ViewData.Model, input.ModelProperty),
                input.WidthPerCent,
                input.Access,
                input.PlaceHolder,
                input.DisabledWhenParentNotSelected,
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
            string triggeredByProperty,
            string ajaxUri,
            string ajaxActionParamName,
            string selectedValue,
            int widthPercent,
            ControlAccess access = ControlAccess.Edit,
            string optionLabel = null,
            bool disabledWhenParentNotSelected = false,
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
            var setDisableString = string.Empty;
            var removeDisabledString = string.Empty;

            if (optionLabel != null)
            {
                htmlAttributes.Add("data-option-lbl", optionLabel);
            }

            if (disabledWhenParentNotSelected)
            {
                htmlAttributes.Add("disabled", "disabled");
                setDisableString = "targetElement.setAttribute('disabled','disabled');";
                removeDisabledString = "targetElement.removeAttribute('disabled');";
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
            scriptBuilder.AppendFormat(Js1CreateInitFunction, 
                inputId, 
                triggeredByProperty, 
                selectedValue, 
                removeDisabledString, 
                optionLblStr, 
                setDisableString,
                widthPercent,
                ajaxUri,
                access == ControlAccess.Edit);
            ApplyCallBeforeDataLoadFunctionString(ref scriptBuilder, ajaxActionParamName, options);
            ApplyJsonToSendString(ref scriptBuilder, ajaxActionParamName, options);
            ApplyRequestString(ref scriptBuilder, inputId, options);
            ApplyOnLoadString(ref scriptBuilder, inputId, options);

            //scriptBuilder.AppendFormat(Js10CallBeforeDataLoadFunctionFormat,
            //  inputId,
            //  triggeredByProperty,
            //  selectedValue,
            //  removeDisabledString,
            //  optionLblStr,
            //  setDisableString,
            //  widthPercent);
            //ApplyCallBeforeDataLoadFunctionString(ref scriptBuilder, ajaxActionParamName, options);
            //ApplyJsonToSendString(ref scriptBuilder, ajaxActionParamName, options);
            //ApplyRequestString(ref scriptBuilder, inputId, options);
            //ApplyOnLoadString(ref scriptBuilder, inputId, options);
            //scriptBuilder.AppendFormat(Js11CallBeforeDataLoadFunctionFormat);



            ////ApplyErrorCallbackString(ref scriptBuilder, options);
            //ApplySendRequestString(ref scriptBuilder, options);
            scriptBuilder.AppendFormat(Js7EndFormat, inputId);
            var script = string.Concat("<script>", scriptBuilder.ToString(), "</script>");

            return new MvcHtmlString(
                string.Concat(
                    string.Format(BuildHtmlPlaceHolder(), defaultLabelHtml.ToString(),
                    defaultControlHtml.ToString(), hiddenControlHtml.ToString()),
                    Environment.NewLine, script));
        }


        private static string BuildHtmlPlaceHolder()
        {
            return @"<div class=""form-group"">{0}<br/>{1}{2}</div>";
        }

        private static void ApplyCallBeforeDataLoadFunctionString(ref StringBuilder builder, string ajaxParam, SourceCallBackOptions options)
        {
            builder.Append(options == null || string.IsNullOrEmpty(options.BeforeDataLoad) ?
                string.Empty :
                string.Format(Js2CallBeforeDataLoadFunctionFormat, ajaxParam, options.BeforeDataLoad));

        }

        private static void ApplyJsonToSendString(ref StringBuilder builder, string ajaxParam, SourceCallBackOptions options)
        {
            builder.Append(options == null || string.IsNullOrEmpty(options.ManipulateDataSend) ?
                string.Format(Js2SimpleGenerateJsonToSendFormat, ajaxParam) :
                string.Format(Js2GenerateJsonToSendFromFunctionFormat, ajaxParam, options.ManipulateDataSend));
        }

        private static void ApplyRequestString(ref StringBuilder builder, string inputId, SourceCallBackOptions options)
        {
            builder.Append(options == null || options.HttpMethod != HttpVerb.POST  ?
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