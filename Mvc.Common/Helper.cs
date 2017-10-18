using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Common
{
    public static class Helper
    {
        /// <summary>
        ///     var name = player.GetAttributeFrom<DisplayAttribute>("PlayerDescription").Name;
        ///     var maxLength = player.GetAttributeFrom<MaxLengthAttribute>("PlayerName").Length;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }



        /// <summary>
        ///    Extension method to get description string for Enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>decription of the enum</returns>
        public static string Description<Enum>(this Enum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        /// <summary>
        ///    Extension method to get description string for Enum
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>decription of the enum</returns>
        public static string Value(this Enum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            ValueAttribute[] attributes = (ValueAttribute[])fi.GetCustomAttributes(
                typeof(ValueAttribute), false);
            if (attributes != null && attributes.Length > 0) return attributes[0].Data;
            else return string.Empty;
        }

        /// <summary>
        ///    Extension method to get display name
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>decription of the enum</returns>
        public static string DisplayName<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DisplayNameAttribute[] attributes = (DisplayNameAttribute[])fi.GetCustomAttributes(
                typeof(DisplayNameAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].DisplayName;
            else
                return string.Empty;
        }


        /// <summary>
        ///    Extension method to get display name
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="source">Enum value</param>
        /// <returns>decription of the enum</returns>
        public static DisplayAttribute Display<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0];
            else
                return new DisplayAttribute();
        }

        public static bool ParseBool<TModel>(TModel value)
        {
            bool result = false;
            if (value == null)
                return result;

            if(bool.TryParse(value.ToString(), out result))
            {
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}
