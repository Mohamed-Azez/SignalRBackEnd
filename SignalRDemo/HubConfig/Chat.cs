using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.HubConfig
{
    //4Tutorial
    public partial class MyHub
    {
        public async Task getOnlineUsers()
        {
            Guid currUserId = _context.Connections.Where(c => c.SignalRid == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
            List<User> onlineUsers = _context.Connections
                .Where(c => c.PersonId != currUserId)
                .Select(c =>
                    new User(c.PersonId, _context.People.Where(p => p.Id == c.PersonId).Select(p => p.Name).SingleOrDefault()!, c.SignalRid)
                ).ToList();
            await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
        }
        public async Task sendMsg(string connId, string msg)
        {
            //await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
            await Clients.Others.SendAsync("sendMsgResponse", Context.ConnectionId, msg);
        }
    }
}
