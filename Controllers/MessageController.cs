
using Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly EcomDbContext _context;

        public MessageController(EcomDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> Get()
        {
            return await _context.Messages.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Message Message)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();

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

            return Ok(new { messages ,Discussion });

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