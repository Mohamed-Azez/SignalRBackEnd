using Microsoft.AspNetCore.SignalR;
namespace SignalRDemo.HubConfig
{
    public class MyHub : Hub
    {
        public async Task AskServer(string someTextFromClient)
        {
            string tempString = string.Empty;
            if (someTextFromClient == "hey")
            {
                tempString = "message was 'het'";
            }
            else
            {
                tempString = "message was something else";
            }
            //difference between client and clients
            //client take once Ip but clients take several devices Ip when add ,
            //it have maximum connection the max is connection 8 if we use client.clients but we can send from hub to all clients with using client.ALl
            await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        }
    }
}
