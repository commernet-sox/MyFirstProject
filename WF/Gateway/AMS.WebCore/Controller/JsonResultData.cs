using CPC;
using Newtonsoft.Json;

namespace AMS.WebCore
{
    public class JsonResultData
    {
        public JsonResultData(ApiCode code=ApiCode.Success, string message = "success", object data = null)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }

        public void SetResult(ApiCode code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        [JsonProperty("code")]
        public ApiCode Code { get; set; } = ApiCode.Success;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
