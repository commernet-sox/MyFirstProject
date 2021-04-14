using Grpc.Net.Client;
using GrpcService;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            while (true)
            {
                var name = Console.ReadLine();
                var reply = await client.SayHelloAsync(
                 new HelloRequest { Name = name });
                Console.WriteLine("Greeter 服务返回数据: " + reply.Message);
                
            }
            
        }
    }
}
