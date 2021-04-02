using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StudyExtend.CSharp8
{
    /// <summary>
    /// 获取数据元素语法糖
    /// </summary>
    public class UseRange
    {
        public static void GetRange()
        {
            var arr = new int[5] { 1,2,3,4,5};
            Console.WriteLine(arr[^1]);

            var arr1 = arr[3..5];
            Console.WriteLine(Tostring( arr1,',','\''));

            var arr2 = arr[1..^1];
            Console.WriteLine(Tostring(arr2, ',', '\''));

            var arr3 = arr[1..];
            Console.WriteLine(Tostring(arr3, ',', '\''));

            var arr4 = arr[..^1];
            Console.WriteLine(Tostring(arr4, ',', '\''));

            var arr5 = arr[..2];
            Console.WriteLine(Tostring(arr5, ',', '\''));
        }
        public static string Tostring(IEnumerable collection, char separator, char quote)
        {
            const char EmptyChar = '\0';
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
    }
}
