using System;
using System.Net.Http;
using AspectCore.DependencyInjection;
using CPC.Http;
using CPC;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HttpClientTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var a = (decimal)DateTime.Now.Day / (decimal)DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)*100;
            //Console.WriteLine(a);
            //FluentClient使用
            //FluentClient fluentClient = new FluentClient(new System.Net.Http.HttpClient(), "http://localhost:5000/WeatherForecast/TestSchoolMapper");
            //RequestOptions requestOptions = new RequestOptions();
            //var res = fluentClient.SendAsync(System.Net.Http.HttpMethod.Get).WithArgument("Name", "111").WithArgument("Desc", "222").WithHeader("accept", "text/plain").AsResponse();
            //var text = res.Result.Message.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(text);

            //HttpClient使用
            //using (var httpClient = new HttpClient())
            //{
            //    string a = "111";
            //    string b = "222";
            //    var result = httpClient.GetAsync($"http://localhost:5000/WeatherForecast/TestSchoolMapper?Name={a}&Desc={b}").Result;
            //    Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            //}

            //IServiceContext services =new ServiceContext();
            //services.AddHttpClient();
            //EngineContext.Initialize(services);
            //var httpClient = EngineContext.Resolve<HttpClient>();
            //using (httpClient)
            //{
            //    string a = "111";
            //    string b = "222";
            //    var result = httpClient.GetAsync($"http://localhost:5000/WeatherForecast/TestSchoolMapper?Name={a}&Desc={b}").Result;
            //    Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            //}

            //FluentClient fluentClient = new FluentClient(new System.Net.Http.HttpClient(), "http://localhost:5000/WeatherForecast/TestSchoolMapper");
            //RequestOptions requestOptions = new RequestOptions();
            //var res = fluentClient.SendAsync(System.Net.Http.HttpMethod.Get).WithArgument("Name", "111").WithArgument("Desc", "222").CreateDownload();
            //res.DownloadAsync();


        }
    }
}
