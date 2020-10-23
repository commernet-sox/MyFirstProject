using Microsoft.AspNetCore.Mvc;
using SDT.BaseTool;
using System.Net;

namespace SDT.Service
{
    public class BodyResult : ObjectResult
    {
        public BodyResult(ApiCode code, string message = "") : this(new Outcome(code, message))
        {

        }

        public BodyResult(Outcome oc) : this(oc.Code, oc)
        {

        }

        public BodyResult(HttpStatusCode code, object value) : base(value) => StatusCode = code.ConvertInt32();

        public BodyResult(ApiCode code, object value) : base(value) => StatusCode = code.ToStatus().ConvertInt32();
    }
}
