﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPC.Http
{
    /// <summary>
    /// defines how the HTTP client should dispatch requests and process responses at a low level, for example to handle transient failures and errors. Only one may be used by the client.
    /// </summary>
    public interface IRequestCoordinator
    {
        /// <summary>
        /// dispatch an HTTP request.
        /// </summary>
        /// <param name="request">the request.</param>
        /// <param name="dispatcher">a method which executes the request.</param>
        /// <returns>the final HTTP response.</returns>
        Task<HttpResponseMessage> ExecuteAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher);
    }
}
