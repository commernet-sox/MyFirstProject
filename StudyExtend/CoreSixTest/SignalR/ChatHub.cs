using Microsoft.AspNetCore.SignalR;

namespace CoreSixTest.SignalR
{
    public class ChatHub:Hub
    {
        /// <summary>
        /// 这里的方法是给前端调用的
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", "连接成功[来自服务器的信息]");
        }
    }
}
