﻿using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Elliptical.Mvc
{
    public static partial class Extensions
    {
        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tail_length"></param>
        /// <returns></returns>
        public static string LastChars(this string source, int tailLength)
        {
            if (tailLength >= source.Length)
                return source;
            return source.Substring(source.Length - tailLength);
        }

        public static string FirstChars(this string source, int startLength)
        {
            return source.Substring(0, startLength);
        }

        public static bool InString(this string source, string value)
        {
            var index = source.IndexOf(value);
            return (index > -1);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string source)
        {
            return Regex.IsMatch(source, @"^\d+$");
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Concatenate(this string source, string str, string separator)
        {
            if (source.Length > 0)
            {
                source = source + separator + str;
            }
            else
            {
                source = str;
            }

            return source;
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string source)
        {
            var myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(source);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToProperCase(this string source)
        {
            const string pattern = @"(?<=\w)(?=[A-Z])";
            var result = Regex.Replace(source, pattern,
                " ", RegexOptions.None);
            return result.Substring(0, 1).ToUpper() + result.Substring(1);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string source)
        {
            source = source.ToTitleCase();
            return source.Substring(0, 1).ToLower() + source.Substring(1);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToWrappedInQuotes(this string value)
        {
            return "\"" + value + "\"";
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToWrappedInSingleQuotes(this string value)
        {
            return "'" + value + "'";
        }

        /// <summary>
        ///     Returns the first part of a split string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="strSeparator"></param>
        /// <returns></returns>
        public static string ToStringFirstPart(this string source, char chrSeparator)
        {
            var parts = source.Split(chrSeparator);
            return parts[0];
        }

        /// <summary>
        ///     Returns the last part of a split string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chrSeparator"></param>
        /// <returns></returns>
        public static string ToStringLastPart(this string source, char chrSeparator)
        {
            var parts = source.Split(chrSeparator);
            var length = parts.Length;
            return parts[length - 1];
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToPhraseCase(this string source)
        {
            return Regex.Replace(source, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        public static string ToDefaultSelectValue(this string source)
        {
            if (source.ToLower() == "select")
            {
                return null;
            }
            return source;
        }

        public static T CopyPropertiesFrom<T>(this T target, object source)
        {
            return copyPropertiesFrom(target, source);
        }

        public static T CopyPropertiesFrom<T>(this T target, object source, bool nullifyDefaultSelectValue)
        {
            if (nullifyDefaultSelectValue)
            {
                return copyPropertiesFromNullifySelect(target, source);
            }
            return copyPropertiesFrom(target, source);
        }

        public static Task<T> CopyPropertiesFromAsync<T>(this T target, object source)
        {
            return Task.Run(() => { return copyPropertiesFrom(target, source); });
        }

        public static Task<T> CopyPropertiesFromAsync<T>(this T target, object source, bool nullifyDefaultSelectValue)
        {
            return Task.Run(() =>
            {
                if (nullifyDefaultSelectValue)
                {
                    return copyPropertiesFromNullifySelect(target, source);
                }
                return copyPropertiesFrom(target, source);
            });
        }

        private static T copyPropertiesFrom<T>(T target, object source)
        {
            var targetType = target.GetType();
            var sourceType = source.GetType();

            var sourceProps = sourceType.GetProperties();
            foreach (var propInfo in sourceProps)
            {
                //Get the matching property from the target
                var toProp =
                    (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name);

                //If it exists and it's writeable
                if (toProp != null && toProp.CanWrite)
                {
                    //Copy non null value from the source to the target
                    var value = propInfo.GetValue(source, null);
                    if(value !=null && value.ToString() !="")
                    {
                        toProp.SetValue(target, value, null);
                    }
                }
            }
            return target;
        }

        private static T copyPropertiesFromNullifySelect<T>(T target, object source)
        {
            var targetType = target.GetType();
            var sourceType = source.GetType();

            var sourceProps = sourceType.GetProperties();
            foreach (var propInfo in sourceProps)
            {
                //Get the matching property from the target
                var toProp =
                    (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name);

                //If it exists and it's writeable
                if (toProp != null && toProp.CanWrite)
                {
                    //Copy the value from the source to the target
                    var value = propInfo.GetValue(source, null);
                    if (value !=null && value.ToString().ToLower() == "select")
                    {
                        value = null;
                    }
                    toProp.SetValue(target, value, null);
                }
            }
            return target;
        }
    }
}