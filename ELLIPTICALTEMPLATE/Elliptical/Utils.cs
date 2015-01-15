using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Text;
using System.Globalization;
using MiscUtil;

namespace Elliptical.Mvc
{
    public static partial class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string StripNewLineChars(string msg)
        {
            msg = msg.Replace('\r', ' ');
            msg = msg.Replace('\n', ' ');
            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            //var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[StaticRandom.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        static Expression StringBuilderAppend(Expression instance, Expression arg)
        {
            var method = typeof(StringBuilder).GetMethod("Append", new Type[] { arg.Type });
            return Expression.Call(instance, method, arg);
        }
        /// <summary>
        /// iterates model properties to output a document string body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string StringBody<T>(T model)
        {
            IEnumerable<T> data = new[] { model };
            var props = typeof(T).GetProperties();
            var sb = Expression.Parameter(typeof(StringBuilder));
            var obj = Expression.Parameter(typeof(T));
            Expression body = sb;
            foreach (var prop in props)
            {
                body = StringBuilderAppend(body, Expression.Constant(prop.Name));
                body = StringBuilderAppend(body, Expression.Constant(":"));
                body = StringBuilderAppend(body, Expression.Constant(Environment.NewLine));
                body = StringBuilderAppend(body, Expression.Property(obj, prop));
                body = StringBuilderAppend(body, Expression.Constant(Environment.NewLine + Environment.NewLine));
            }
            body = Expression.Call(body, "AppendLine", Type.EmptyTypes);
            var lambda = Expression.Lambda<Func<StringBuilder, T, StringBuilder>>(body, sb, obj);
            var func = lambda.Compile();

            var result = new StringBuilder();
            foreach (T row in data)
            {
                func(result, row);
            }
            return result.ToString();
        }

       

       

        /// <summary>
        /// redirect to login page with returnUrl querystring of current path and query
        /// </summary>
        public static string LoginRedirectUrl(HttpContextBase context)
        {
            string path = context.Request.Url.PathAndQuery;
            path = context.Server.UrlEncode(path);
            string loginUrl = System.Web.Security.FormsAuthentication.LoginUrl;
            string redirectUrl = path + "?ReturnUrl=" + path;
            return redirectUrl;
        }

        

    }

   
}