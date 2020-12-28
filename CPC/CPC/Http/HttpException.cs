using System.Net;
using System.Net.Http;

namespace CPC.Http
{
    /// <summary>
    /// represents an error returned by the upstream server.
    /// </summary>
    public class HttpException : System.Exception
    {
        #region Members
        /// <summary>
        /// the HTTP status of the response.
        /// </summary>
        public HttpStatusCode Status { get; }

        /// <summary>
        /// the HTTP response which caused the exception.
        /// </summary>
        public IResponse Response { get; }

        /// <summary>
        /// the HTTP response message which caused the exception.
        /// </summary>
        public HttpResponseMessage ResponseMessage { get; }
        #endregion

        #region Constructors
        /// <summary>construct an instance.</summary>
        /// <param name="response">the HTTP response which caused the exception.</param>
        /// <param name="message">the error message that explains the reason for the exception.</param>
        /// <param name="innerException">the exception that is the cause of the current exception (or <c>null</c> for no inner exception).</param>
        public HttpException(IResponse response, string message, System.Exception innerException = null)
            : base(message, innerException)
        {
            Response = response;
            ResponseMessage = response.Message;
            Status = response.Message.StatusCode;
        }
        #endregion
    }
}
