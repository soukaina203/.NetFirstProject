// using Microsoft.AspNetCore.Mvc;
// using signals;

// public class ChatController : ControllerBase
// {
//     private readonly ChatHub _chatHub;

//     public ChatController(ChatHub chatHub)
//     {
//         _chatHub = chatHub;
//     }

//     [HttpPost("/messages")]
//     public async Task SendMessage([FromBody] string message)
//     {
//         await _chatHub.SendMessage(message);
//     }
// }
