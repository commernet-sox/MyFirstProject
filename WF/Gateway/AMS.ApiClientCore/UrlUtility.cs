using System.Collections.Generic;

namespace CPC.Http
{
    public class UrlUtility
    {
        public static string Format(string url)
        {
            url = url.Replace(@"\", "/");
            return url;
        }

        public static string Combine(params string[] urls)
        {
            var urlcs = new List<string>();
            foreach (var url in urls)
            {
                if (url.IsNull() || url == "/" || url == @"\")
                {
                    continue;
                }
                var urlc = Format(url).TrimStart('/').TrimEnd('/');
                urlcs.Add(urlc);
            }
            return urlcs.JoinStr("/");
        }

    }
}
