using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPC
{
    public static class StringExtension
    {
        public const char EmptyChar = '\0';

        #region Trim Extension
        /// <summary>
        /// 去前后空格；为null返回""
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TrimEx(this string source)
        {
            if (source == null)
            {
                return "";
            }

            return source.Trim();
        }

        /// <summary>
        /// 截断数据到指定长度，汉字或ASCII字符皆占一位
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static string TrimByUnicodeLengh(this string source, int length, bool ellipsis = true)
        {
            if (length <= 0)
            {
                throw new InvalidOperationException("截取长度太小");
            }

            source = source.TrimEx();
            if (source.Length <= length)
            {
                return source;
            }

            var ellFlag = "...";

            if (ellipsis)
            {
                if (length - ellFlag.Length <= 0)
                {
                    throw new InvalidOperationException("截取长度太小");
                }

                return source.Substring(0, length - ellFlag.Length) + ellFlag;
            }

            return source.Substring(0, length);
        }

        /// <summary>
        /// 截断数据到指定长度，汉字占两位，ASCII字符占一位
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static string TrimByLengh(this string source, int length, bool ellipsis = true)
        {
            if (length <= 0)
            {
                throw new InvalidOperationException("截取长度太小");
            }

            var ellFlag = "...";
            source = source.TrimEx();
            var len = source.Length;
            if (len == 0)
            {
                return "";
            }
            if (ellipsis)
            {
                if (length - ellFlag.Length <= 0)
                {
                    throw new InvalidOperationException("截取长度太小");
                }
            }

            return TruncateCore(source, length, ellipsis ? ellFlag : "");
        }

        private static string TruncateCore(string str, int len, string ellipsisFlag)
        {
            var l = str.Length;

            for (var i = 0; i < l && i < len; i++)
            {
                if (str[i] > 0xFF)
                {
                    len--;
                }
            }

            if (l <= len)
            {
                return str;
            }

            if (ellipsisFlag.Length > 0)
            {
                //退位置
                var pos = ellipsisFlag.Length;
                while (pos > 0)
                {
                    if (str[len] > 0xFF)
                    {
                        len--;
                        pos -= 2;
                    }
                    else
                    {
                        len--;
                        pos -= 1;
                    }
                }

                return str.Substring(0, len) + ellipsisFlag;
            }

            return str.Substring(0, len);
        }
        #endregion

        /// <summary>
        /// 将集合（数组、List等）转换成字符串
        /// </summary>
        /// <param name="collection">集合</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字符串，如1,2,3</returns>
        public static string ToString(this IEnumerable collection, char separator) => ToString(collection, separator, EmptyChar);

        /// <summary>
        /// 将集合（数组、List等）转换成字符串
        /// </summary>
        /// <param name="collection">集合</param>
        /// <param name="separator">分隔符</param>
        /// <param name="quote">引用符</param>
        /// <returns>字符串，如'1','2','3'</returns>
        public static string ToString(this IEnumerable collection, char separator, char quote)
        {
            var result = new StringBuilder();
            var i = 0;
            var quoteStr = (quote == EmptyChar) ? string.Empty : quote.ToString();
            foreach (var obj in collection)
            {
                var value = quoteStr + obj.ToString() + quoteStr;
                if (i++ == 0)
                {
                    result.Append(value);
                }
                else
                {
                    result.Append(separator);
                    result.Append(value);
                }
            }
            if (result.Length == 0 && quote != EmptyChar)
            {
                result.AppendFormat("{0}{0}", quote);
            }

            var rtn = result.ToString();
            result.Length = 0;
            return rtn;
        }

        /// <summary>
        /// 将集合（数组、List等）转换成字符串
        /// </summary>
        /// <param name="collection">集合</param>
        /// <param name="toStringFunc">ToString</param>
        /// <param name="separator">分隔符</param>
        /// <returns>字符串，如'1','2','3'</returns>
        public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> toStringFunc, char separator) => ToString(collection, toStringFunc, separator, EmptyChar);

        /// <summary>
        /// 将集合（数组、List等）转换成字符串
        /// </summary>
        /// <param name="collection">集合</param>
        /// <param name="toStringFunc">ToString</param>
        /// <param name="separator">分隔符</param>
        /// <param name="quote">引用符</param>
        /// <returns>字符串，如'1','2','3'</returns>
        public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> toStringFunc, char separator, char quote)
        {
            if (collection == null)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            var i = 0;
            var quoteStr = (quote == EmptyChar) ? string.Empty : quote.ToString();
            foreach (var obj in collection)
            {
                var value = toStringFunc(obj);
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                value = quoteStr + value + quoteStr;
                if (i++ == 0)
                {
                    result.Append(value);
                }
                else
                {
                    result.Append(separator);
                    result.Append(value);
                }
            }
            if (result.Length == 0 && quote != EmptyChar)
            {
                result.AppendFormat("{0}{0}", quote);
            }

            var rtn = result.ToString();
            result.Length = 0;
            return rtn;
        }
    }
}
