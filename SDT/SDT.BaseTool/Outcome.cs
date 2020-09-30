using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public class Outcome
    {
        public Outcome(ApiCode code = ApiCode.Success, string message = "")
        {
            Code = code;
            Message = message;
        }

        public ApiCode Code { get; set; }

        public string Message { get; set; }
    }
    public class Outcome<T> : Outcome
    {
        [JsonConstructor]
        public Outcome(ApiCode code = ApiCode.Success, string message = "", T data = default) : base(code, message) => Data = data;

        public Outcome(T data)
        {
            Code = ApiCode.Success;
            Message = string.Empty;
            Data = data;
        }

        public T Data { get; set; }
    }

    public class PageData<T>
    {
        [JsonConstructor]
        public PageData()
        {

        }

        public PageData(int pageIndex, int pageSize, int total, T data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Data = data;
        }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public T Data { get; set; }
    }

    public class OutcomePage<T> : Outcome<T>
    {
        [JsonConstructor]
        public OutcomePage(ApiCode code = ApiCode.Success, string message = "") : base(code, message)
        {

        }

        public OutcomePage(int pageIndex, int pageSize, int total, T data) : base(data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
        }

        public int Total { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
