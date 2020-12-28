using System.Net;

namespace SDT.Redis
{
    internal static class EndPointExtensions
    {
        internal static string GetFriendlyName(this EndPoint endPoint)
        {
            if (endPoint is DnsEndPoint dnsEndPoint)
            {
                return $"{dnsEndPoint.Host}:{dnsEndPoint.Port}";
            }


            if (endPoint is IPEndPoint ipEndPoint)
            {
                return $"{ipEndPoint.Address}:{ipEndPoint.Port}";
            }

            return endPoint.ToString();
        }
    }
}
