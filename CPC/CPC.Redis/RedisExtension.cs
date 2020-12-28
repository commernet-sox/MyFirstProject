using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPC.Redis
{
    internal static class RedisExtension
    {
        internal static T ToDataEx<T>(this RedisValue aim)
        {
            if (aim.IsNullOrEmpty)
            {
                return default;
            }

            var type = typeof(T);
            if (type.IsValueType || type == typeof(string))
            {
                var objStr = Encoding.UTF8.GetString(aim);
                objStr = $"\"{objStr}\"";
                return JsonExtensions.ToDataEx<T>(objStr);
            }

            return JsonExtensions.DeserializeEx<T>(aim);
        }

        internal static RedisValue ToDataEx<T>(this T aim)
        {
            if (aim.IsNull())
            {
                return string.Empty;
            }

            var type = aim.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return Encoding.UTF8.GetBytes(aim.ConvertString());
            }
            return JsonExtensions.SerializeEx(aim);
        }

        internal static List<string> ToListEx(this IEnumerable<RedisKey> aim) => aim.Select(t => t.ConvertString()).ToList();

        internal static List<T> ToListEx<T>(this IEnumerable<RedisValue> aim) => aim.Select(t => t.ToDataEx<T>()).ToList();

        internal static List<RedisKey> ToListEx(this IEnumerable<string> aim) => aim.Select(t => (RedisKey)t).ToList();

        internal static RedisKey[] ToArrayEx(this IEnumerable<string> aim) => aim.Select(t => (RedisKey)t).ToArray();

        internal static RedisValue[] ToArrayEx<T>(this IEnumerable<T> aim) => aim.Select(t => t.ToDataEx<T>()).ToArray();

        internal static HashSet<T> ToSetEx<T>(this IEnumerable<T> aim) => new HashSet<T>(aim);

    }
}
