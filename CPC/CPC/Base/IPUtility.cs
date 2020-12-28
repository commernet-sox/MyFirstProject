using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace CPC
{
    public sealed class IPUtility
    {
        #region Private Members
        /// <summary>
        /// A类: 10.0.0.0-10.255.255.255
        /// </summary>
        private static readonly long IpABegin, IpAEnd;

        /// <summary>
        /// B类: 172.16.0.0-172.31.255.255   
        /// </summary>
        private static readonly long IpBBegin, IpBEnd;

        /// <summary>
        /// C类: 192.168.0.0-192.168.255.255
        /// </summary>
        private static readonly long IpCBegin, IpCEnd;
        #endregion

        #region Constructors
        /// <summary>
        /// static new
        /// </summary>
        static IPUtility()
        {
            IpABegin = ConvertToNumber("10.0.0.0");
            IpAEnd = ConvertToNumber("10.255.255.255");

            IpBBegin = ConvertToNumber("172.16.0.0");
            IpBEnd = ConvertToNumber("172.31.255.255");

            IpCBegin = ConvertToNumber("192.168.0.0");
            IpCEnd = ConvertToNumber("192.168.255.255");
        }
        #endregion

        #region Methods
        /// <summary>
        /// ipaddress convert to long
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static long ConvertToNumber(string ipAddress) => ConvertToNumber(ParseIp(ipAddress));
        /// <summary>
        /// ipaddress convert to long
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static long ConvertToNumber(IPAddress ipAddress)
        {
            var bytes = ipAddress.GetAddressBytes();
            return bytes[0] * 256 * 256 * 256 + bytes[1] * 256 * 256 + bytes[2] * 256 + bytes[3];
        }

        /// <summary>
        /// true表示为内网IP
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIntranet(string ipAddress) => IsIntranet(ConvertToNumber(ipAddress));

        /// <summary>
        /// true表示为内网IP
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIntranet(IPAddress ipAddress) => IsIntranet(ConvertToNumber(ipAddress));

        /// <summary>
        /// true表示为内网IP
        /// </summary>
        /// <param name="longIP"></param>
        /// <returns></returns>
        private static bool IsIntranet(long longIP) => (longIP >= IpABegin) && (longIP <= IpAEnd) ||
                    (longIP >= IpBBegin) && (longIP <= IpBEnd) ||
                    (longIP >= IpCBegin) && (longIP <= IpCEnd);

        /// <summary>
        /// 获取本机内网IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLocalIntranetIP()
        {
            var list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var child in list)
            {
                if (IsIntranet(child))
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// URL转换地址
        /// </summary>
        /// <param name="addressWithPort"></param>
        /// <returns></returns>
        public static EndPoint TryParseEndPoint(string addressWithPort)
        {
            var portStr = string.Empty;
            if (string.IsNullOrEmpty(addressWithPort))
            {
                return null;
            }

            var num = addressWithPort.LastIndexOf(':');
            string host;
            if (num > 0)
            {
                var num2 = addressWithPort.LastIndexOf(']');
                if (num2 > 0)
                {
                    host = addressWithPort.Substring(1, num2);
                    if (num2 < num)
                    {
                        portStr = addressWithPort.Substring(num + 1);
                    }
                }
                else
                {
                    var num3 = addressWithPort.IndexOf(':');
                    if (num3 != num)
                    {
                        host = addressWithPort;
                    }
                    else
                    {
                        host = addressWithPort.Substring(0, num3);
                        portStr = addressWithPort.Substring(num3 + 1);
                    }
                }
            }
            else
            {
                return null;
            }

            var port = portStr.ConvertInt32();
            if (port <= 0)
            {
                return null;
            }

            if (IPAddress.TryParse(host, out var ip))
            {
                return new IPEndPoint(ip, port);
            }

            return new DnsEndPoint(host, port);
        }

        /// <summary>
        /// 获取本机内网IP列表
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetLocalIntranetIPList()
        {
            var list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            var result = new List<IPAddress>();
            foreach (var child in list)
            {
                if (IsIntranet(child))
                {
                    result.Add(child);
                }
            }

            return result;
        }

        public static IPAddress ParseIp(string ipAddress)
        {
            //remove port number from ip address if any
            ipAddress = ipAddress.Split(',').First().Trim();

            var portDelimiterPos = ipAddress.LastIndexOf(":", StringComparison.CurrentCultureIgnoreCase);
            var ipv6WithPortStart = ipAddress.StartsWith("[");
            var ipv6End = ipAddress.IndexOf("]");

            if (portDelimiterPos != -1
                && portDelimiterPos == ipAddress.IndexOf(":", StringComparison.CurrentCultureIgnoreCase)
                || ipv6WithPortStart && ipv6End != -1 && ipv6End < portDelimiterPos)
            {
                ipAddress = ipAddress.Substring(0, portDelimiterPos);
            }

            return IPAddress.Parse(ipAddress);
        }
        #endregion

        #region Ip Range
        public static bool ContainsIp(string rule, string clientIp)
        {
            var ip = ParseIp(clientIp);

            var range = new IpAddressRange(rule);

            if (range.Contains(ip))
            {
                return true;
            }

            return false;
        }

        public static bool ContainsIp(List<string> ipRules, string clientIp)
        {
            var ip = ParseIp(clientIp);

            if (ipRules != null && ipRules.Any())
            {
                foreach (var rule in ipRules)
                {
                    var range = new IpAddressRange(rule);

                    if (range.Contains(ip))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool ContainsIp(List<string> ipRules, string clientIp, out string rule)
        {
            rule = null;
            var ip = ParseIp(clientIp);

            if (ipRules != null && ipRules.Any())
            {
                foreach (var r in ipRules)
                {
                    var range = new IpAddressRange(r);

                    if (range.Contains(ip))
                    {
                        rule = r;
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region IPEndPoint
        public static bool TryParse(string s, out IPEndPoint result)
        {
            var addressLength = s.Length;  // If there's no port then send the entire string to the address parser
            var lastColonPos = s.LastIndexOf(':');

            // Look to see if this is an IPv6 address with a port.
            if (lastColonPos > 0)
            {
                if (s[lastColonPos - 1] == ']')
                {
                    addressLength = lastColonPos;
                }
                // Look to see if this is IPv4 with a port (IPv6 will have another colon)
                else if (s.Substring(0, lastColonPos).LastIndexOf(':') == -1)
                {
                    addressLength = lastColonPos;
                }
            }

            if (IPAddress.TryParse(s.Substring(0, addressLength), out var address))
            {
                uint port = 0;
                if (addressLength == s.Length ||
                    (uint.TryParse(s.Substring(addressLength + 1), NumberStyles.None, CultureInfo.InvariantCulture, out port) && port <= 0xFFFF))
                {
                    result = new IPEndPoint(address, (int)port);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static IPEndPoint Parse(string s)
        {
            if (TryParse(s, out var result))
            {
                return result;
            }

            throw new FormatException("bad IPEndPoint");
        }
        #endregion
    }

    /// <summary>
    /// IP v4 and v6 range helper by jsakamoto
    /// Fork from https://github.com/jsakamoto/ipaddressrange
    /// </summary>
    /// <example>
    /// "192.168.0.0/24" 
    /// "fe80::/10" 
    /// "192.168.0.0/255.255.255.0" 
    /// "192.168.0.0-192.168.0.255"
    /// </example>
    public class IpAddressRange
    {
        public IPAddress Begin { get; set; }

        public IPAddress End { get; set; }

        public IpAddressRange()
        {
            Begin = new IPAddress(0L);
            End = new IPAddress(0L);
        }

        public IpAddressRange(string ipRangeString)
        {
            // remove all spaces.
            ipRangeString = ipRangeString.Replace(" ", "");

            // Pattern 1. CIDR range: "192.168.0.0/24", "fe80::/10"
            var m1 = Regex.Match(ipRangeString, @"^(?<adr>[\da-f\.:]+)/(?<maskLen>\d+)$", RegexOptions.IgnoreCase);
            if (m1.Success)
            {
                var baseAdrBytes = IPAddress.Parse(m1.Groups["adr"].Value).GetAddressBytes();
                var maskBytes = Bits.GetBitMask(baseAdrBytes.Length, int.Parse(m1.Groups["maskLen"].Value));
                baseAdrBytes = Bits.And(baseAdrBytes, maskBytes);
                Begin = new IPAddress(baseAdrBytes);
                End = new IPAddress(Bits.Or(baseAdrBytes, Bits.Not(maskBytes)));
                return;
            }

            // Pattern 2. Uni address: "127.0.0.1", ":;1"
            var m2 = Regex.Match(ipRangeString, @"^(?<adr>[\da-f\.:]+)$", RegexOptions.IgnoreCase);
            if (m2.Success)
            {
                Begin = End = IPAddress.Parse(ipRangeString);
                return;
            }

            // Pattern 3. Begin end range: "169.258.0.0-169.258.0.255"
            var m3 = Regex.Match(ipRangeString, @"^(?<begin>[\da-f\.:]+)-(?<end>[\da-f\.:]+)$", RegexOptions.IgnoreCase);
            if (m3.Success)
            {
                Begin = IPAddress.Parse(m3.Groups["begin"].Value);
                End = IPAddress.Parse(m3.Groups["end"].Value);
                return;
            }

            // Pattern 4. Bit mask range: "192.168.0.0/255.255.255.0"
            var m4 = Regex.Match(ipRangeString, @"^(?<adr>[\da-f\.:]+)/(?<bitmask>[\da-f\.:]+)$", RegexOptions.IgnoreCase);
            if (m4.Success)
            {
                var baseAdrBytes = IPAddress.Parse(m4.Groups["adr"].Value).GetAddressBytes();
                var maskBytes = IPAddress.Parse(m4.Groups["bitmask"].Value).GetAddressBytes();
                baseAdrBytes = Bits.And(baseAdrBytes, maskBytes);
                Begin = new IPAddress(baseAdrBytes);
                End = new IPAddress(Bits.Or(baseAdrBytes, Bits.Not(maskBytes)));
                return;
            }

            throw new FormatException("Unknown IP range string.");
        }

        public bool Contains(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily != Begin.AddressFamily)
            {
                return false;
            }

            var adrBytes = ipAddress.GetAddressBytes();
            return Bits.GE(Begin.GetAddressBytes(), adrBytes) && Bits.LE(End.GetAddressBytes(), adrBytes);
        }

        public bool Contains(string ip) => Contains(IPUtility.ParseIp(ip));
    }

    internal static class Bits
    {
        internal static byte[] Not(byte[] bytes) => bytes.Select(b => (byte)~b).ToArray();

        internal static byte[] And(byte[] pa, byte[] pb) => pa.Zip(pb, (a, b) => (byte)(a & b)).ToArray();

        internal static byte[] Or(byte[] pa, byte[] pb) => pa.Zip(pb, (a, b) => (byte)(a | b)).ToArray();

        internal static bool GE(byte[] pa, byte[] pb) => pa.Zip(pb, (a, b) => a == b ? 0 : a < b ? 1 : -1)
                .SkipWhile(c => c == 0)
                .FirstOrDefault() >= 0;

        internal static bool LE(byte[] pa, byte[] pb) => pa.Zip(pb, (a, b) => a == b ? 0 : a < b ? 1 : -1)
                .SkipWhile(c => c == 0)
                .FirstOrDefault() <= 0;

        internal static byte[] GetBitMask(int sizeOfBuff, int bitLen)
        {
            var maskBytes = new byte[sizeOfBuff];
            var bytesLen = bitLen / 8;
            var bitsLen = bitLen % 8;
            for (var i = 0; i < bytesLen; i++)
            {
                maskBytes[i] = 0xff;
            }

            if (bitsLen > 0)
            {
                maskBytes[bytesLen] = (byte)~Enumerable.Range(1, 8 - bitsLen).Select(n => 1 << n - 1).Aggregate((a, b) => a | b);
            }

            return maskBytes;
        }
    }
}
