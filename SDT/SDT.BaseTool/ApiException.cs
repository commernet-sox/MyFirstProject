using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public class ApiException : Exception
    {
        public object Body { get; }

        public ApiCode Code { get; }

        public ApiException(ApiCode code, string message) : this(new Outcome(code, message))
        {

        }

        public ApiException(Outcome oc) : this(oc.Code, oc)
        {
            Code = oc.Code;
            Body = oc;
        }

        public ApiException(ApiCode code, object data) : base()
        {
            Code = code;
            Body = data;
        }
    }
}
