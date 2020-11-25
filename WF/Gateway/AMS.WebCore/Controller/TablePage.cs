using Newtonsoft.Json;

namespace AMS.WebCore
{
    public class TablePage
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("rows")]
        public object Rows { get; set; }
    }
}
