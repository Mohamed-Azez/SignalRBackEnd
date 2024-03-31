using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SignalRDemo.HubConfig
{
    public partial class MyHub : Hub
    {
        private readonly SignalrContext _context;
        public MyHub(SignalrContext context)
        {
            _context = context;
        }

        //4Tutorial
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Guid currUserId = _context.Connections.Where(c => c.SignalRid == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
            //_context.Connections.RemoveRange(_context.Connections.Where(p => p.PersonId == currUserId).ToList());
            //_context.SaveChanges();
            Clients.Others.SendAsync("userOff", currUserId);
            return base.OnDisconnectedAsync(exception);
        }
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
        public async Task authMe(PersonInfo personInfo)
        {
            string currSignalrID = Context.ConnectionId;
            Person tempPerson = _context.People.Where(p => p.UserName == personInfo.userName && p.Password == personInfo.password).SingleOrDefault()!;

            if (tempPerson != null) //if credentials are correct
            {
                Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

                //Connection currUser = new Connection
                //{
                //    PersonId = tempPerson.Id,
                //    SignalRid = currSignalrID,
                //    TimeStamp = DateTime.Now
                //};
                //await _context.Connections.AddAsync(currUser);
                //await _context.SaveChangesAsync();

                User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
                await Clients.Caller.SendAsync("authMeResponseSuccess", newUser);//4Tutorial
                await Clients.Others.SendAsync("userOn", newUser);//4Tutorial
            }

            else //if credentials are incorrect
            {
                await Clients.Caller.SendAsync("authMeResponseFail");
            }
        }
        //3Tutorial
        public async Task reauthMe(Guid personId)
        {
            string currSignalrID = Context.ConnectionId;
            Person tempPerson = _context.People.Where(p => p.Id == personId).SingleOrDefault()!;

            if (tempPerson != null) //if credentials are correct
            {
                Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

                //Connection currUser = new Connections
                //{
                //    PersonId = tempPerson.Id,
                //    SignalrId = currSignalrID,
                //    TimeStamp = DateTime.Now
                //};
                //await ctx.Connections.AddAsync(currUser);
                //await ctx.SaveChangesAsync();

                User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
                await Clients.Caller.SendAsync("reauthMeResponse", newUser);//4Tutorial
                await Clients.Others.SendAsync("userOn", newUser);//4Tutorial
            }
        } //end of reauthMe


        //4Tutorial
        public void logOut(Guid personId)
        {
            //_context.Connections.RemoveRange(_context.Connections.Where(p => p.PersonId == personId).ToList());
            //_context.SaveChanges();
            Clients.Caller.SendAsync("logoutResponse");
            Clients.Others.SendAsync("userOff", personId);
        }
    }
}
