using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace dotnet.Models
{
    public class ChatHub :Hub
    {
        // public override async Task OnConnectedAsync(){
        //     await Clients.All.SendAsync("RecieveMessage");

        // }
        // send to all connected users
        public async Task SendMessage(Message message){
         await Clients.All.SendAsync("RecieveMessage",message);
        }
        
        
                public string GetConnectionId() => Context.ConnectionId;

    }
}