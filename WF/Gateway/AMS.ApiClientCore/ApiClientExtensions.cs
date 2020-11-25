using System.Data;
using System.Threading.Tasks;

namespace CPC.Http
{
    public static class ApiClientExtensions
    {
        #region IApiClient
        public static Task<IResponse> GetAsync(this IApiClient client, string url, object arguments = null) => client.CreateClient(url).GetAsync().WithArguments(arguments).AsResponse();

        public static Task<IResponse> PostAsync<T>(this IApiClient client, string url, T body) => client.CreateClient(url).PostAsync(body).AsResponse();

        public static Task<IResponse> PutAsync(this IApiClient client, string url, object arguments = null) => client.CreateClient(url).PutAsync().WithArguments(arguments).AsResponse();

        public static Task<IResponse> DeleteAsync(this IApiClient client, string url, object arguments = null) => client.CreateClient(url).DeleteAsync().WithArguments(arguments).AsResponse();

        public static IResponse Get(this IApiClient client, string url, object arguments = null) => client.GetAsync(url, arguments).Result;

        public static IResponse Post<T>(this IApiClient client, string url, T body) => client.PostAsync(url, body).Result;

        public static IResponse Put(this IApiClient client, string url, object arguments = null) => client.PutAsync(url, arguments).Result;

        public static IResponse Delete(this IApiClient client, string url, object arguments = null) => client.DeleteAsync(url, arguments).Result;
        #endregion

        #region Result
        public static async Task<Outcome<T>> ResultAsync<T>(this IResponse response)
        {
            var oc = new Outcome<T>
            {
                Code = response.Status.ToCode()
            };

            if (response.IsSuccessStatusCode)
            {
                oc.Data = await response.As<T>();
            }
            else
            {
                var outcome = await response.As<Outcome>();
                if (outcome != null)
                {
                    oc.Code = outcome.Code;
                    oc.Message = outcome.Message;
                }
                else
                {
                    oc.Message = await response.AsString();
                }
            }

            return oc;
        }

        public static Outcome<T> Result<T>(this IResponse response) => response.ResultAsync<T>().Result;

        public static async Task<Outcome<T>> ResultAsync<T>(this IRequest request)
        {
            var response = await request.AsResponse();
            return await response.ResultAsync<T>();
        }

        public static Outcome<T> Result<T>(this IRequest request) => request.ResultAsync<T>().Result;
        #endregion
    }
}
