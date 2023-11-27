
using Context;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly EcomDbContext _context;
        private readonly IHubContext<ChatHub> _chatHub;


        public MessageController(EcomDbContext context, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            _chatHub = chatHub;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> Get()
        {
            return await _context.Messages.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Message model)
        {
            _context.Messages.Add(model);
            await _context.SaveChangesAsync();

            var connectionId = ChatHub.userConnectionIds.Where(e => e.Key ==
             model.IdReceiver).Select(e => e.Value).FirstOrDefault();

            if (connectionId != null)
            {
                await _chatHub.Clients.Client(connectionId)
                .SendAsync("RecieveMessage", model);
            }
            int senderId = model.IdSender;
            string senderConnectionId;
            if (ChatHub.userConnectionIds.TryGetValue(senderId, out senderConnectionId))
            {
                // Send a copy of the message to the sender
                await _chatHub.Clients.Client(senderConnectionId).SendAsync("RecieveMessage", model);
            }


            return Ok();
        }


        // PUT: api/Messages/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> Update([FromForm] Message Message)
        {


            _context.Entry(Message).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Done";
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<(IEnumerable<Message>, Discussion)>> GetMessegeDiscussion(int id)
        {
            var messages = await _context.Messages.Where(m => m.IdDiscussion == id).ToListAsync();
            var Discussion = await _context.Discussions.FindAsync(id);

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(new { messages, Discussion });

        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var MessageToDelete = await _context.Messages.FindAsync(id);
            if (MessageToDelete == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(MessageToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }

}