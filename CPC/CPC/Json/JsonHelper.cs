using Newtonsoft.Json;
using System;
using System.Text;

namespace CPC
{
    /// <summary>
    /// json.net operate
    /// </summary>
    public class JsonHelper
    {
        #region Members
        public static JsonSerializerSettings CommonSetting { get; private set; }
        #endregion

        #region Constructors
        static JsonHelper() => CommonSetting = new JsonSerializerSettings
        {
            //remove a circular reference
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //ignore miss member
            MissingMemberHandling = MissingMemberHandling.Ignore,
            //the first letter lowercase
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //datetime format：yyyy-MM-dd
            DateFormatString = "yyy-MM-dd HH:mm:ss",
            NullValueHandling = NullValueHandling.Include,
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };
        #endregion

        public static void Customize(Action<JsonSerializerSettings> action) => action?.Invoke(CommonSetting);

        /// <summary>
        /// object to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string Serialize<T>(T value, Formatting formatting = Formatting.None)
        {
            var settings = CommonSetting;
            return Serialize(value, settings, formatting);
        }

        /// <summary>
        /// object to json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string Serialize<T>(T value, JsonSerializerSettings settings, Formatting formatting = Formatting.None) => JsonConvert.SerializeObject(value, formatting, settings);

        /// <summary>
        /// json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            var settings = CommonSetting;
            return Deserialize<T>(value, settings);
        }

        /// <summary>
        /// json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value, JsonSerializerSettings settings) => JsonConvert.DeserializeObject<T>(value, settings);
    }

    /// <summary>
    /// json extension method
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// object to json string
        /// </summary>
        /// <param name="aim"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ToJsonEx<T>(this T aim, JsonSerializerSettings settings = null)
        {
            settings = settings ?? JsonHelper.CommonSetting;
            return JsonHelper.Serialize(aim, settings);
        }

        public static byte[] SerializeEx<T>(this T aim, JsonSerializerSettings settings = null)
        {
            settings = settings ?? JsonHelper.CommonSetting;
            var objStr = ToJsonEx(aim, settings);
            return Encoding.UTF8.GetBytes(objStr);
        }

        /// <summary>
        /// json string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aim"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToDataEx<T>(this string aim, JsonSerializerSettings settings = null)
        {
            settings = settings ?? JsonHelper.CommonSetting;
            return JsonHelper.Deserialize<T>(aim, settings);
        }

        public static T DeserializeEx<T>(this byte[] aim, JsonSerializerSettings settings = null)
        {
            settings = settings ?? JsonHelper.CommonSetting;
            var objStr = Encoding.UTF8.GetString(aim);
            return ToDataEx<T>(objStr, settings);
        }

    }
}
