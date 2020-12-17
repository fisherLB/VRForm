using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphicsEvaluatePlatform.WebPlatform
{
    public interface IClient
    {
        void addMessage(object message);
    }
    [HubName("myChatHub")]
    public class LetsChat : Hub<IClient>
    {

        public void send(string message)
        {
            Clients.All.addMessage(message);
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            var id = Context.ConnectionId;
            var c = Clients.Caller;
            return base.OnConnected();
        }
        public static void notify(object message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LetsChat, IClient>();
            context.Clients.All.addMessage(message);
        }
    }
    [HubName("broadcastMessage")]
    public class SignalrServerToClient
    {
        static readonly IHubContext _myHubContext = GlobalHost.ConnectionManager.GetHubContext<LetsChat>();
        public static void BroadcastMessage(object obj)
        {
            _myHubContext.Clients.All.addMessage(obj);
        }
    }
}