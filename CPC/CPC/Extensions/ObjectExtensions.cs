using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace CPC
{
    /// <summary>
    /// static extension
    /// </summary>
    public static class ObjectExtensions
    {
        #region Common Convertsion
        /// <summary>
        /// convert to string
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ConvertString(this object aim, string defaultValue = "")
        {
            if (aim == null || aim == DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                return aim + "";
            }
        }

        public static byte ConvertByte(this string aim, byte defaultValue = 0)
        {
            if (byte.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int32
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertInt32(this string aim, int defaultValue = 0)
        {
            if (int.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int32
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertInt32(this long aim, int defaultValue = 0)
        {
            if (aim <= int.MaxValue && aim >= int.MinValue)
            {
                return Convert.ToInt32(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int32
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertInt32(this double aim, int defaultValue = 0)
        {
            if (aim <= int.MaxValue && aim >= int.MinValue)
            {
                return Convert.ToInt32(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int32
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertInt32(this float aim, int defaultValue = 0)
        {
            if (aim <= int.MaxValue && aim >= int.MinValue)
            {
                return Convert.ToInt32(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int32
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ConvertInt32(this decimal aim, int defaultValue = 0)
        {
            if (aim <= int.MaxValue && aim >= int.MinValue)
            {
                return Convert.ToInt32(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int64
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ConvertInt64(this string aim, long defaultValue = 0)
        {
            if (long.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int64
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ConvertInt64(this double aim, int defaultValue = 0)
        {
            if (aim <= long.MaxValue && aim >= long.MinValue)
            {
                return Convert.ToInt64(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int64
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ConvertInt64(this float aim, int defaultValue = 0)
        {
            if (aim <= long.MaxValue && aim >= long.MinValue)
            {
                return Convert.ToInt64(aim);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to int64
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ConvertInt64(this decimal aim, int defaultValue = 0)
        {
            if (aim <= long.MaxValue && aim >= long.MinValue)
            {
                return Convert.ToInt64(aim);
            }
            else
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// convert to int16
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short ConvertInt16(this string aim, short defaultValue = 0)
        {
            if (short.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to double
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ConvertDouble(this string aim, double defaultValue = 0)
        {
            if (double.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to specified number of double
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="digits"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ConvertDouble(this string aim, int digits, double defaultValue = 0) => Math.Round(ConvertDouble(aim, defaultValue), digits);

        /// <summary>
        /// convert to float(注意Float精确度问题，谨顺使用此类型)
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ConvertFloat(this string aim, float defaultValue = 0)
        {
            if (float.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to decimal
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ConvertDecimal(this string aim, decimal defaultValue = 0)
        {
            if (decimal.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to bool
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ConvertBool(this string aim, bool defaultValue = false)
        {
            if (bool.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// convert to datetime
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTime(this string aim, DateTime defaultValue = default)
        {
            if (DateTime.TryParse(aim, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// dbnull convert null
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object IfDbNullThenNull(this object item)
        {
            if (item == DBNull.Value)
            {
                item = null;
            }

            return item;
        }
        #endregion

        #region Data Manipulation
        /// <summary>
        /// convert to datatable  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <returns></returns>
        [Obsolete("不推荐使用，后续将删除")]
        public static DataTable ToDataTable<T>(this IEnumerable<T> aim) where T : class
        {
            var list = aim;

            var dt = new DataTable();
            //get the aim object type
            
            PropertyInfo[] plist = null;

            foreach (var item in list)
            {
                if (dt.Columns.Count == 0)
                {
                    //get list model properties
                    plist = typeof(T).GetProperties();

                    //add property name to columns
                    foreach (var proper in plist)
                    {
                        var colType = proper.PropertyType;
                        if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dt.Columns.Add(proper.Name, colType);
                    }
                }

                var dr = dt.NewRow();
                //fill data to datarow
                foreach (var proper in plist)
                {
                    dr[proper.Name] = proper.GetValue(item, null) ?? DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return dt;
        }

        /// <summary>
        /// convert to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <returns></returns>
        [Obsolete("不推荐使用，后续将删除")]
        public static List<T> ToList<T>(this DataTable aim) where T : class, new()
        {
            var list = new List<T>();

            var plist = new List<PropertyInfo>();

            foreach (var item in typeof(T).GetProperties())
            {
                if (aim.Columns.Contains(item.Name))
                {
                    plist.Add(item);
                }
            }

            //put the median of the data table into List
            foreach (DataRow item in aim.Rows)
            {
                var t = new T();
                foreach (var proper in plist)
                {
                    if (item[proper.Name] != DBNull.Value)
                    {
                        proper.SetValue(t, item[proper.Name], null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// data paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> ListPage<T>(this IQueryable<T> aim, int pageIndex, int pageSize)
        {
            if (aim == null)
            {
                return default;
            }

            return aim.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// data paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IQueryable<T> ListPage<T>(this IQueryable<T> aim, int pageIndex, int pageSize, out int totalCount)
        {
            if (aim == null)
            {
                totalCount = 0;
                return default;
            }

            totalCount = aim.Count();
            return aim.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// data paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> ListPage<T>(this IEnumerable<T> aim, int pageIndex, int pageSize)
        {
            if (aim == null)
            {
                return default;
            }

            return aim.Skip(pageIndex * pageSize).Take(pageSize);
        }

        /// <summary>
        /// data paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> ListPage<T>(this IEnumerable<T> aim, int pageIndex, int pageSize, out int totalCount)
        {
            if (aim == null)
            {
                totalCount = 0;
                return default;
            }

            totalCount = aim.Count();
            return aim.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static PageData<IEnumerable<T>> Page<T>(this IQueryable<T> aim, int pageIndex, int pageSize)
        {
            var data = aim.ListPage(pageIndex, pageSize, out var total).ToList();
            return new PageData<IEnumerable<T>>(pageIndex, pageSize, total, data);
        }

        public static PageData<IEnumerable<T>> Page<T>(this IEnumerable<T> aim, int pageIndex, int pageSize)
        {
            var data = aim.ListPage(pageIndex, pageSize, out var total).ToList();
            return new PageData<IEnumerable<T>>(pageIndex, pageSize, total, data);
        }

        /// <summary>
        /// get the key/value arguments from an object representation.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetKeyValueArguments(this object arguments)
        {
            if (arguments == null)
            {
                return Enumerable.Empty<KeyValuePair<string, object>>();
            }

            return (
                from property in arguments.GetType().GetRuntimeProperties()
                where property.CanRead && property.GetIndexParameters().Any() != true
                select new KeyValuePair<string, object>(property.Name, property.GetValue(arguments))
            ).ToArray();
        }
        #endregion

        #region Other
        public static bool In<T>(this T aim, params T[] list)
        {
            if (list.IsNull())
            {
                return false;
            }

            return list.Contains(aim);
        }

        public static bool In<T>(this T aim, IEqualityComparer<T> comparer, params T[] list)
        {
            if (list.IsNull())
            {
                return false;
            }

            return list.Contains(aim, comparer);
        }

        public static bool EqualsEx(this string aim, string value)
        {
            var a = aim.IsNull();
            var b = value.IsNull();

            if (a | b)
            {
                if (a & b)
                {
                    return true;
                }

                return false;
            }

            return aim.ToLowerInvariant() == value.ToLowerInvariant();
        }

        public static string JoinStr<T>(this IEnumerable<T> aim, string separator = ",")
        {
            if (aim.IsNull())
            {
                return string.Empty;
            }

            return string.Join(separator, aim);
        }


        public static int[] FindArrayIndex<T>(this IEnumerable<T> aim, Predicate<T> match)
        {
            var res = new List<int>();
            var i = 0;
            foreach (var item in aim)
            {
                if (match.Invoke(item))
                {
                    res.Add(i);
                }
                i++;
            }
            return res.ToArray();
        }

        /// <summary>
        /// determine whether the value is null or not
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull(this string aim) => string.IsNullOrWhiteSpace(aim);

        /// <summary>
        /// determine whether the value is null or not
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull(this object aim) => aim == null || aim == DBNull.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T? aim) where T : struct
        {
            if (aim.HasValue)
            {
                return aim.GetValueOrDefault().Equals(default(T));
            }
            return true;
        }

        /// <summary>
        /// determine whether the value is null or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this IEnumerable<T> aim) => aim == null || aim.Count() == 0;

        /// <summary>
        /// determine whether the value is null or not
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull(this DataTable aim) => aim == null || aim.Rows.Count == 0;

        /// <summary>
        /// determine whether the value is null or not
        /// </summary>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static bool IsNull(this DataSet aim) => aim == null || aim.Tables.Count == 0 || aim.Tables[0].IsNull();

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();

            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Returns true if the type is one of the built in simple types.
        /// </summary>
        public static bool IsSimpleType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0]; // or  Nullable.GetUnderlyingType(type)
            }

            if (type.IsEnum)
            {
                return true;
            }

            if (type == typeof(Guid))
            {
                return true;
            }

            var tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Char:
                case TypeCode.String:
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                    return true;
                case TypeCode.Object:
                    return (typeof(TimeSpan) == type) || (typeof(DateTimeOffset) == type);
                default:
                    return false;
            }
        }

        /// <summary>
        /// add raw arguments to the URI's query string.
        /// </summary>
        /// <param name="uri">the URI to extend.</param>
        /// <param name="arguments">the raw arguments to add.</param>
        /// <param name="ignoreNullArguments">whether to ignore arguments with a null value.</param>
        /// <remarks>This method can't use 【System.Net.Http.UriExtensions.ParseQueryString】 because it isn't compatible with portable class libraries.</remarks>
        public static Uri WithArguments(this Uri uri, bool ignoreNullArguments, params KeyValuePair<string, object>[] arguments)
        {
            // concat new arguments
            var newQueryString = string.Join("&",
                from argument in arguments
                where !ignoreNullArguments || argument.Value != null
                let key = StringUtility.UrlEncode(argument.Key)
                let value = argument.Value != null ? StringUtility.UrlEncode(argument.Value.ToString()) : string.Empty
                select key + "=" + value
            );
            if (string.IsNullOrWhiteSpace(newQueryString))
            {
                return uri;
            }

            // adjust URL
            var builder = new UriBuilder(uri);
            builder.Query = !string.IsNullOrWhiteSpace(builder.Query)
                ? builder.Query.TrimStart('?') + "&" + newQueryString
                : newQueryString;

            return builder.Uri;
        }
        #endregion

        #region Enum
        public static int ConvertInt32<T>(this T aim) where T : struct, Enum => Convert.ToInt32(aim);

        public static T ConvertEnum<T>(this int aim) where T : struct, Enum
        {
            var result = (T)Enum.ToObject(typeof(T), aim);
            return result;
        }

        public static T ConvertEnum<T>(this string aim, bool ignoreCase = true, T defaultValue = default) where T : struct, Enum
        {
            if (Enum.TryParse<T>(aim, ignoreCase, out var result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static HttpStatusCode ToStatus(this ApiCode code)
        {
            HttpStatusCode status;

            switch (code)
            {
                case ApiCode.Success:
                    status = HttpStatusCode.OK;
                    break;
                case ApiCode.SystemBusy:
                    status = HttpStatusCode.InternalServerError;
                    break;
                default:
                    {
                        var num = code.ConvertInt32();
                        if (num % 10 == 0)
                        {
                            status = (num / 10).ConvertEnum<HttpStatusCode>();
                        }
                        else
                        {
                            status = HttpStatusCode.BadRequest;
                        }
                        break;
                    }
            }
            return status;
        }

        public static ApiCode ToCode(this HttpStatusCode status)
        {
            if (status == HttpStatusCode.OK)
            {
                return ApiCode.Success;
            }

            var code = (status.ConvertInt32() * 10).ConvertEnum<ApiCode>();
            return code;
        }

        public static string GetTypeName(this Type type)
        {
            var fullyQualifiedTypeName = type.AssemblyQualifiedName;

            var builder = new StringBuilder();

            // loop through the type name and filter out qualified assembly details from nested type names
            var writingAssemblyName = false;
            var skippingAssemblyDetails = false;
            for (var i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                var current = fullyQualifiedTypeName[i];
                switch (current)
                {
                    case '[':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(current);
                        break;
                    case ']':
                        writingAssemblyName = false;
                        skippingAssemblyDetails = false;
                        builder.Append(current);
                        break;
                    case ',':
                        if (!writingAssemblyName)
                        {
                            writingAssemblyName = true;
                            builder.Append(current);
                        }
                        else
                        {
                            skippingAssemblyDetails = true;
                        }
                        break;
                    default:
                        if (!skippingAssemblyDetails)
                        {
                            builder.Append(current);
                        }
                        break;
                }
            }

            return builder.ToString();
        }
        #endregion

        #region DataSetExtenstion
        public static bool Merge(this DataSet dsDest, string tableName, DataSet dsSrc, MissingSchemaAction msAction)
        {
            if ((dsSrc == null) || (dsDest == null))
            {
                return false;
            }

            if ((dsSrc.Tables[tableName] == null) || (dsDest.Tables[tableName] == null))
            {
                return false;
            }

            dsDest.Tables[tableName].Clear();
            dsDest.Tables[tableName].AcceptChanges();

            dsDest.Tables[tableName].Merge(dsSrc.Tables[tableName], true, msAction);

            return true;
        }
        public static bool Merge(this DataSet dsDest, string tableName, DataSet dsSrc) => Merge(dsDest, tableName, dsSrc, MissingSchemaAction.Ignore);

        /// <summary>
        /// 结构完全一样时，建议用这个方法替换Merge方法；效率更好
        /// </summary>
        /// <param name="dsDest"></param>
        /// <param name="tableName"></param>
        /// <param name="dsSrc"></param>
        public static void Import(this DataSet dsDest, string tableName, DataSet dsSrc)
        {
            var destTable = dsDest.Tables[tableName];
            destTable.Clear();
            destTable.AcceptChanges();

            destTable.BeginLoadData();
            foreach (DataRow rawRow in dsSrc.Tables[tableName].Rows)
            {
                destTable.ImportRow(rawRow);
            }
            destTable.EndLoadData();
            destTable.AcceptChanges();
        }

        public static void CopyRow(this DataRow destRow, DataRow origRow)
        {
            var rowtp = destRow.GetType();
            var piarray = rowtp.GetProperties();
            for (var i = 0; i < piarray.Length; i++)
            {
                var pi = piarray[i];

                try
                {
                    var ovalue = pi.GetValue(origRow, null);

                    pi.SetValue(destRow, ovalue, null);
                }
                catch { continue; }
            }
        }
        #endregion

        #region Reflection
        public static bool IsConcrete(this Type type) => !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;

        public static void CopyToChild<TParent, TChild>(this TParent parent, TChild child) where TChild : TParent
        {
            var parentType = typeof(TParent);
            var properties = parentType.GetProperties();
            foreach (var property in properties)
            {
                //循环遍历属性
                if (property.CanRead && property.CanWrite)
                {
                    //进行属性拷贝
                    property.SetValue(child, property.GetValue(parent, null), null);
                }
            }
        }

        public static void CopyToParent<TChild, TParent>(this TChild child, TParent parent) where TChild : TParent
        {
            var parentType = typeof(TParent);
            var properties = parentType.GetProperties();
            foreach (var property in properties)
            {
                //循环遍历属性
                if (property.CanRead && property.CanWrite)
                {
                    //进行属性拷贝
                    property.SetValue(parent, property.GetValue(child, null), null);
                }
            }
        }

        public static void SetProperty<T>(this T aim, string name, object value)
        {
            if (name.IsNull())
            {
                return;
            }

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == name)
                {
                    if (property.CanWrite)
                    {
                        //进行属性拷贝
                        property.SetValue(aim, value, null);
                    }

                    break;
                }
            }
        }

        public static void SetProperty<T>(this T aim, IDictionary<string, object> pairs)
        {
            if (pairs.IsNull())
            {
                return;
            }

            var total = pairs.Count;
            var cur = 0;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (pairs.TryGetValue(property.Name, out var value))
                {
                    if (property.CanWrite)
                    {
                        //进行属性拷贝
                        property.SetValue(aim, value, null);
                    }
                    cur++;
                    if (cur >= total)
                    {
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
