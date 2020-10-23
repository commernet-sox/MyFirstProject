using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    /// <summary>
    /// a middleware class which can intercept and modify HTTP requests and responses. This can be used to implement common authentication, error-handling, etc.
    /// </summary>
    public interface IHttpFilter
    {
        /// <summary>
        /// method invoked just before the HTTP request is submitted. This method can modify the outgoing HTTP request.
        /// </summary>
        /// <param name="request">the HTTP request.</param>
        void OnRequest(IRequest request);

        /// <summary>
        /// method invoked just after the HTTP response is received. This method can modify the incoming HTTP response.
        /// </summary>
        /// <param name="response">the HTTP response.</param>
        /// <param name="httpErrorAsException">whether HTTP error responses (e.g. HTTP 404) should be raised as exceptions.</param>
        void OnResponse(IResponse response, bool httpErrorAsException);
    }
}
