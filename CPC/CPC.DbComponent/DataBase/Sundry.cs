using System;

namespace CPC.DbComponent
{
    public static class Sundry
    {
        // Fields
        private static IDbUtil database;

        internal static IDbUtil IdataBaseClient
        {
            get
            {
                if (database == null)
                {
                    throw new Exception("没有数据库连接");
                }
                return database;
            }
            set => database = value;
        }

        private static char bit4ToChar(int i)
        {
            if (i < 10)
            {
                return (char)(0x30 + i);
            }
            if (i < 0x10)
            {
                i -= 10;
                return (char)(0x41 + i);
            }
            return '\0';
        }

        public static string bytesToHexString(object obj, DataBaseType dBType)
        {
            if ((obj == DBNull.Value) || (obj == null))
            {
                return "null";
            }
            var buffer = (byte[])obj;
            var chArray = new char[buffer.Length * 2];
            for (var i = 0; i < buffer.Length; i++)
            {
                chArray[i * 2] = bit4ToChar(buffer[i] >> 4);
                chArray[(i * 2) + 1] = bit4ToChar(buffer[i] & 15);
            }
            if (dBType == DataBaseType.OracleDBType)
            {
                return ("HEXTORAW('" + new string(chArray) + "')");
            }
            return ("0x" + new string(chArray));
        }

        public static int HexToInt(string s)
        {
            if (s == null)
            {
                s = "";
            }
            var chArray = s.ToUpper().ToCharArray();
            var num = 0;
            for (var i = 0; i < chArray.Length; i++)
            {
                num = (num << 4) + ((chArray[i] < 'A') ? (chArray[i] - '0') : ((chArray[i] - 'A') + 10));
            }
            return num;
        }

        public static string RowValueToString(object obj, DataBaseType dBType)
        {
            if ((obj == DBNull.Value) || (obj == null))
            {
                return "null";
            }
            switch (obj.GetType().FullName)
            {
                case "System.String":
                case "System.Guid":
                    {
                        return ("'" + obj.ToString().Replace("'", "''") + "'");
                    }
                case "System.TimeSpan":
                    {
                        if (dBType == DataBaseType.SQLDBType)
                        {
                            var ts = (TimeSpan)obj;
                            return string.Format("'{0}:{1}:{2}.{3}'", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                        }
                        return ("'" + obj.ToString().Replace("'", "''") + "'");
                    }

                case "System.DateTime":
                    {
                        var time = (DateTime)obj;
                        if (dBType == DataBaseType.OracleDBType)
                        {
                            return string.Format("to_date('{0}-{1}-{2} {3}:{4}:{5}','yyyy-mm-dd hh24:mi:ss')",
                                                 new object[]
                                                     {
                                                         time.Year, time.Month, time.Day, time.Hour, time.Minute,
                                                         time.Second
                                                     });
                        }
                        else if (dBType == DataBaseType.OleDbDBType)
                        {
                            return string.Format("'{0}-{1}-{2} {3}:{4}:{5}'",
                                            new object[]
                                                 {
                                                     time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second
                                                 });
                        }
                        return string.Format("'{0}-{1}-{2} {3}:{4}:{5}.{6}'",
                                             new object[]
                                                 {
                                                     time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second,
                                                     time.Millisecond.ToString("000")
                                                 });
                    }
                case "System.Byte[]":
                    {
                        return bytesToHexString(obj, dBType);
                    }
                case "System.Boolean"://只针对bit类型
                    {
                        return obj.Equals(true) ? "1" : "0";
                    }
            }
            return obj.ToString();
        }

        // Properties
    }
}