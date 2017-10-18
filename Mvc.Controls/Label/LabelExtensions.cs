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
    public static class LabelExtensions
    {
        public static MvcHtmlString LabelForModel<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //}            

            return CreateLabelFor(html, labelText, htmlFieldName, htmlAttributes);
        }       


        public static MvcHtmlString LabelFor(this HtmlHelper html,
            string labelText, string htmlFieldName, object htmlAttributes)
        {
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //}

            return CreateLabelFor(html, labelText, htmlFieldName, htmlAttributes);
        }



        private static MvcHtmlString CreateLabelFor(HtmlHelper html,
           string labelText, string htmlFieldName, object htmlAttributes)
        {
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //}

            var htmlAttributesRoute = htmlAttributes != null
               ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
               : new RouteValueDictionary();

            string defaultClass = "";

            //Add the default class : label-primary
            if (htmlAttributesRoute["class"] == null)
            {
                htmlAttributesRoute.Add("class", defaultClass);
            }
            else
            {
                htmlAttributesRoute["class"] = defaultClass + " " + htmlAttributesRoute["class"];
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributesRoute);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }



        public static MvcHtmlString LabelModel<TModel, TValue>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //} 
            return CreateLabel(html, labelText, htmlFieldName, htmlAttributes);
        }

        //public static MvcHtmlString Label(this HtmlHelper html,
        //   string labelText, IDictionary<string, object> htmlAttributes)
        //{
        //    //if (String.IsNullOrEmpty(labelText))
        //    //{
        //    //    return MvcHtmlString.Empty;
        //    //}

        //    return CreateLabel(html, labelText, string.Empty, htmlAttributes);
        //}


        public static MvcHtmlString Label(this HtmlHelper html,
           string labelText, string id, IDictionary<string, object> htmlAttributes)
        {
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //}

            return CreateLabel(html, labelText, id, htmlAttributes);
        }


        private static MvcHtmlString CreateLabel(this HtmlHelper html,
          string labelText, string id, object htmlAttributes)
        {
            //if (String.IsNullOrEmpty(labelText))
            //{
            //    return MvcHtmlString.Empty;
            //}
                       

            var htmlAttributesRoute = htmlAttributes != null
               ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
               : new RouteValueDictionary();

            string defaultClass = "labelText";


            if (!String.IsNullOrEmpty(id))
            {
                htmlAttributesRoute.Add("id", id);
                htmlAttributesRoute.Add("name", id);
            }

            //Add the default class : label-primary
            if (htmlAttributesRoute["class"] == null)
            {
                htmlAttributesRoute.Add("class", defaultClass);
            }
            else
            {
                htmlAttributesRoute["class"] = defaultClass + " " + htmlAttributesRoute["class"];
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributesRoute);
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}
