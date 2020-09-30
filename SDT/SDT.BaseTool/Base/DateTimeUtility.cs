using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SDT.BaseTool
{
    public static class DateTimeUtility
    {
        #region ServerTime
        /// <summary>
        /// 服务器时间，因存在获取方式问题，会存在一定的误差
        /// </summary>
        public static DateTime Now => DateTime.Now.Add(_timeDifference);

        private static TimeSpan _timeDifference = TimeSpan.FromSeconds(0);

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SyncServerTime(DateTime serverDatetime) => _timeDifference = serverDatetime - DateTime.Now;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SyncServerTime(Func<DateTime> getServerTime)
        {
            var sw = Stopwatch.StartNew();
            var serverDatetime = getServerTime();
            sw.Stop();
            _timeDifference = serverDatetime - DateTime.Now.AddMilliseconds(-sw.ElapsedMilliseconds);
        }

        public static long Timestamp(DateTime time)
        {
            var expiresAtOffset = new DateTimeOffset(time);
            var totalSeconds = expiresAtOffset.ToUnixTimeMilliseconds();
            return totalSeconds;
        }
        #endregion

        #region Lunar Calendar Correlation
        /// <summary>
        /// 中国日历
        /// </summary>
        private static readonly Lazy<ChineseLunisolarCalendar> _china = new Lazy<ChineseLunisolarCalendar>();

        /// <summary>
        /// get lunar year
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns>result</returns>
        public static string GetLunarYear(DateTime time)
        {
            var yearIndex = _china.Value.GetSexagenaryYear(time);
            var celestial = "甲乙丙丁戊己庚辛壬癸";
            var branch = "子丑寅卯辰巳午未申酉戌亥";
            var animal = "鼠牛虎兔龙蛇马羊猴鸡狗猪";
            var year = _china.Value.GetYear(time);
            var celestialNumber = _china.Value.GetCelestialStem(yearIndex);
            var branchNumber = _china.Value.GetTerrestrialBranch(yearIndex);
            var result = string.Format("[{1}]{2}{3}{0}", year, animal[branchNumber - 1], celestial[celestialNumber - 1], branch[branchNumber - 1]);
            return result;
        }

        /// <summary>
        /// get lunar month
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns>result</returns>
        public static string GetLunarMonth(DateTime time)
        {
            var year = _china.Value.GetYear(time);
            var month = _china.Value.GetMonth(time);
            var leapMonth = _china.Value.GetLeapMonth(year);

            if (leapMonth != 0 && month >= leapMonth)
            {
                month--;
            }

            var monthHead = "正二三四五六七八九十";
            var isLeapMonth = month == leapMonth;
            var result = isLeapMonth ? "闰" : "";

            if (month <= 10)
            {
                result += monthHead[month - 1];
            }
            else if (month == 11)
            {
                result += "十一";
            }
            else
            {
                result += "腊";
            }

            result += "月";
            return result;
        }

        /// <summary>
        /// get lunar day
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns>result</returns>
        public static string GetLunarDay(DateTime time)
        {
            var day = _china.Value.GetDayOfMonth(time);
            var dayDecade = "初十廿三";
            var dayUnits = "一二三四五六七八九十";
            string result;

            if (day == 20)
            {
                result = "二十";
            }
            else if (day == 30)
            {
                result = "三十";
            }
            else
            {
                result = dayDecade[(day - 1) / 10].ToString();
                result += dayUnits[(day - 1) % 10];
            }

            return result;
        }

        /// <summary>
        /// get solar term
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns></returns>
        public static string GetSolarTerm(DateTime time)
        {
            var solarterms = new string[] { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };

            var solartermsData = new int[] { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

            var dtBase = new DateTime(1900, 1, 6, 2, 5, 0);
            var result = "";

            for (var i = 1; i <= 24; i++)
            {
                var num = (525948.76 * (time.Year - 1900)) + solartermsData[i - 1];
                var dtNew = dtBase.AddMinutes(num);

                if (dtNew.DayOfYear == time.DayOfYear)
                {
                    result = solarterms[i - 1];
                }
            }

            return result;
        }

        /// <summary>
        /// get solar holiday
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns>result</returns>
        public static string GetSolarHoliday(DateTime time)
        {
            var solarHoliday = new Dictionary<string, string>
            {
                //公历节日添加
                { "0101", "元旦" },
                { "0214", "情人节" },
                { "0305", "雷锋日" },
                { "0308", "妇女节" },
                { "0312", "植树节" },
                { "0315", "消权日" },
                { "0401", "愚人节" },
                { "0501", "劳动节" },
                { "0504", "青年节" },
                { "0601", "儿童节" },
                { "0701", "建党节" },
                { "0801", "建军节" },
                { "0910", "教师节" },
                { "1001", "国庆节" },
                { "1224", "平安夜" },
                { "1225", "圣诞节" }
            };

            var key = time.Month.ToString("00") + time.Day.ToString("00");
            if (solarHoliday.ContainsKey(key))
            {
                return solarHoliday[key];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// get china holiday
        /// </summary>
        /// <param name="time">gregorian calendar</param>
        /// <returns>result</returns>
        public static string GetChinaHoliday(DateTime time)
        {
            var lunarHoliday = new Dictionary<string, string>
            {
                //农历节日添加
                { "0101", "春节" },
                { "0115", "元宵节" },
                { "0505", "端午节" },
                { "0815", "中秋节" },
                { "0909", "重阳节" },
                { "1208", "腊八节" }
            };

            var result = string.Empty;
            var year = _china.Value.GetYear(time);
            var month = _china.Value.GetMonth(time);
            var leapMonth = _china.Value.GetLeapMonth(year);
            var day = _china.Value.GetDayOfMonth(time);

            if (_china.Value.GetDayOfYear(time) == _china.Value.GetDaysInYear(year))
            {
                result = "除夕";
            }
            else if (leapMonth != month)
            {
                if (leapMonth != 0 && month >= leapMonth)
                {
                    month--;
                }

                var key = month.ToString("00") + day.ToString("00");
                if (lunarHoliday.ContainsKey(key))
                {
                    var holiday = lunarHoliday[key];
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = holiday;
                    }
                    else
                    {
                        result += " " + holiday;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Common
        #region Members
        private static int lastTicks = -1;
        private static DateTime lastDateTime;

        /// <summary>
        /// unix下的纪元时间
        /// </summary>
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// the max milliseconds since epoch.
        /// </summary>
        private static readonly long DateTimeMaxValueMillisecondsSinceEpoch =
            (DateTime.MaxValue - UnixEpoch).Ticks / 10000;
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the current utc time in an optimized fashion.
        /// </summary>
        public static DateTime LocalUtcNow
        {
            get
            {
                var tickCount = Environment.TickCount;
                if (tickCount == lastTicks)
                {
                    return lastDateTime;
                }

                var dt = DateTime.UtcNow;
                lastTicks = tickCount;
                lastDateTime = dt;
                return dt;
            }
        }

        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }
            else
            {
                if (dateTime == DateTime.MinValue)
                {
                    return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
                }
                else if (dateTime == DateTime.MaxValue)
                {
                    return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
                }
                else
                {
                    return dateTime.ToUniversalTime();
                }
            }
        }

        /// <summary>
        /// Converts a DateTime to number of milliseconds since Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>Number of seconds since Unix epoch.</returns>
        public static long ToMillisecondsSinceEpoch(DateTime dateTime) => (ToUniversalTime(dateTime) - UnixEpoch).Ticks / 10000;
        /// <summary>
        /// Converts a DateTime to number of seconds since Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>Number of seconds since Unix epoch.</returns>
        public static long ToSecondsSinceEpoch(DateTime dateTime) => ToMillisecondsSinceEpoch(dateTime) / 1000;
        /// <summary>
        /// Converts from number of milliseconds since Unix epoch to DateTime.
        /// </summary>
        /// <param name="millisecondsSinceEpoch">The number of milliseconds since Unix epoch.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime ToDateTimeFromMillisecondsSinceEpoch(long millisecondsSinceEpoch)
        {
            // MaxValue has to be handled specially to avoid rounding errors
            if (millisecondsSinceEpoch == DateTimeMaxValueMillisecondsSinceEpoch)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return UnixEpoch.AddTicks(millisecondsSinceEpoch * 10000);
            }
        }

        /// <summary>
        /// Converts from number of seconds since Unix epoch to DateTime.
        /// </summary>
        /// <param name="secondsSinceEpoch">The number of seconds since Unix epoch.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime ToDateTimeFromSecondsSinceEpoch(long secondsSinceEpoch) => ToDateTimeFromMillisecondsSinceEpoch(secondsSinceEpoch * 1000);

        /// <summary>
        /// Converts a DateTime to local time (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <param name="kind">A DateTimeKind.</param>
        /// <returns>The DateTime in local time.</returns>
        public static DateTime ToLocalTime(DateTime dateTime, DateTimeKind kind)
        {
            if (dateTime.Kind == kind)
            {
                return dateTime;
            }
            else
            {
                if (dateTime == DateTime.MinValue)
                {
                    return DateTime.SpecifyKind(DateTime.MinValue, kind);
                }
                else if (dateTime == DateTime.MaxValue)
                {
                    return DateTime.SpecifyKind(DateTime.MaxValue, kind);
                }
                else
                {
                    return DateTime.SpecifyKind(dateTime.ToLocalTime(), kind);
                }
            }
        }

        /// <summary>
        /// SpecifyKind
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DateTime SpecifyKind(DateTime dt, DateTimeKind kind)
        {
            if (dt.Kind == kind)
            {
                return dt;
            }

            return DateTime.SpecifyKind(dt, kind);
        }
        #endregion
        #endregion
    }
}
