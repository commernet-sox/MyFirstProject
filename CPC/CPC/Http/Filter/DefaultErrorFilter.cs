namespace CPC.Http
{
    /// <summary>
    /// an HTTP filter which detects failed HTTP requests and throws an exception.
    /// </summary>
    public class DefaultErrorFilter : IHttpFilter
    {
        public void OnRequest(IRequest request) { }

        public void OnResponse(IResponse response, bool httpErrorAsException)
        {
            if (httpErrorAsException && !response.Message.IsSuccessStatusCode)
            {
                throw new HttpException(response, $"the API query failed with status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}");
            }
        }
    }
}
