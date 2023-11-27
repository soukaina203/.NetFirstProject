using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace dotnet.Models
{
    [Authorize]
    public class ChatHub : Hub
    {


        public static readonly Dictionary<int, string> userConnectionIds = new Dictionary<int, string>();

        public override async Task OnConnectedAsync()

        {
            
                int userId = int.Parse(Context.User?.Identity?.Name);

            if (!userConnectionIds.ContainsKey(userId))
            {
                userConnectionIds.Add(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

public async Task SendToUser(int receiverId, Message message)
{
    if (userConnectionIds.TryGetValue(receiverId, out string receiverConnectionId))
    {
        await Clients.Client(receiverConnectionId).SendAsync("RecieveMessage", message);

        // Get the sender's connection ID
        int senderId = message.IdSender;
        string senderConnectionId;
        if (userConnectionIds.TryGetValue(senderId, out senderConnectionId))
        {
            // Send a copy of the message to the sender
            await Clients.Client(senderConnectionId).SendAsync("RecieveMessage", message);
        }
    }
}


        // public async Task SendMessage(Message message)
        // {
        //     await Clients.All.SendAsync("RecieveMessage", message);
        // }

    public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                int idUser = int.Parse(Context.User?.Identity?.Name);

                userConnectionIds.Remove(idUser);

                await base.OnDisconnectedAsync(exception);
            }
            catch (System.Exception e)
            {
                await Clients.All.SendAsync("InnerException", e.InnerException);
            }
        }


    }
}