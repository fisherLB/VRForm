using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GraphicsEvaluatePlatform.WebPlatform
{
    public class EchoConnection : PersistentConnection
    {
        /// <summary>
        /// 当前连接数
        /// </summary>
        private static int _connections = 0;
        /// <summary>
        /// 连接建立时执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        protected override async Task OnConnected(IRequest request, string connectionId)
        {
            Interlocked.Increment(ref _connections);
            await Connection.Send(connectionId, "Hi, " + connectionId + "!");
            await Connection.Broadcast("新连接 " + connectionId + "开启. 当前连接数: " + _connections);

        }
        /// <summary>
        /// 连接关闭时执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        protected Task OnDisconnected(IRequest request, string connectionId)
        {
            Interlocked.Decrement(ref _connections);
            return Connection.Broadcast(connectionId + " 连接关闭. 当前连接数: " + _connections);
        }
        /// <summary>
        /// 服务器接收到前台发送的消息时执行 发送请求 connection.send("信息");
        /// </summary>
        /// <param name="request"></param>
        /// <param name="connectionId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var message = connectionId + ">> " + data;
            return Connection.Broadcast(message);
        }

    }
}