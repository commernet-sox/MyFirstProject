namespace CPC.Http
{
    /// <summary>options for a request.</summary>
    public class RequestOptions
    {
        /// <summary>
        /// whether to ignore null arguments when the request is dispatched.
        /// </summary>
        public bool? IgnoreNullArguments { get; set; }

        /// <summary>
        /// whether HTTP error responses (e.g. HTTP 404) should be ignored (else raised as exceptions).
        /// </summary>
        public bool? IgnoreHttpErrors { get; set; }
    }
}
