﻿using Mvc.Controls.DataTable.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Mvc.Controls
{
    internal static class ControlHelpers
    {
        internal static string GetElementIdFromExpression<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
        }


        internal static string GetElementIdFromExpression<TModel>(this HtmlHelper<TModel> htmlHelper, string fieldName)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName);
        }



        internal static string GetElementNameFromExpression<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        }

        internal static string GetElementNameFromExpression<TModel>(this HtmlHelper<TModel> htmlHelper, string fieldName)
        {
            return htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
        }


        internal static string GetElementIdFromHtmlAttributes(object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                var properties = htmlAttributes.GetType().GetProperties();
                var prop = properties.FirstOrDefault(p => p.Name.ToUpperInvariant() == "ID");
                if (prop != null)
                {
                    return prop.GetValue(htmlAttributes, null).ToString();
                }
            }
            return null;
        }



        internal static string GetPropStringValue<TModel, TProp>(TModel src, Expression<Func<TModel, TProp>> expression)
        {
            Func<TModel, TProp> func = expression.Compile();
            var selectedValString = string.Empty;
            if (src != null)
            {
                TProp propVal = func(src);
                string defaultValString = typeof(TProp).IsValueType && Nullable.GetUnderlyingType(typeof(TProp)) == null
                    ? Activator.CreateInstance(typeof(TProp)).ToString()
                    : string.Empty;
                if ((defaultValString != string.Empty && propVal.ToString() != defaultValString) ||
                    (defaultValString == string.Empty && propVal != null))
                {
                    selectedValString = propVal.ToString();
                }
            }
            return selectedValString;
        }


        internal static string GetPropStringValue(object src, string propName)
        {
            string stringVal = null;
            if (src != null)
            {
                object propVal = src.GetType().GetProperty(propName).GetValue(src, null);
                stringVal = propVal != null ? propVal.ToString() : null;
            }
            return stringVal;
        }


        internal static string ToTitleCase(this string input)
        {
            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(input);
        }

        internal static string ToTitleCaseFromPascal(this string input)
        {
            

            return string.IsNullOrEmpty(input) ? string.Empty :
                input == input.ToUpper() ? input :
                Regex.Replace(input, "(\\B[A-Z])", " $1"); 
        }




        internal static EditControl AssignEditControl(Type type)
        {
            EditControl output = EditControl.Default;
             
            if (type == null || type.GetTypeInfo().IsEnum)
            {
                return output;
            }

            switch (Type.GetTypeCode(type))
            {

                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    output = EditControl.Decimal;
                    break;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    output = EditControl.Numeric;
                    break;
                case TypeCode.String:
                case TypeCode.Char:
                    output = EditControl.TextBox;
                    break;
                case TypeCode.Boolean:
                    output = EditControl.CheckBox;
                    break;
                case TypeCode.DateTime:
                    output = EditControl.Date;
                    break;
                case TypeCode.Object:
                    if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        AssignEditControl(Nullable.GetUnderlyingType(type));
                    }
                    break;
                default:
                    output = EditControl.Default;
                    break;
            }
            return output;
        }


    }
}
