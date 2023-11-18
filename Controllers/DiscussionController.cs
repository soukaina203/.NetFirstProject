using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscussionController : ControllerBase
    {
        private readonly EcomDbContext _context;

        public DiscussionController(EcomDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discussion>>> Get()
        {
            return await _context.Discussions.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Discussion Discussion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Discussions.Add(Discussion);
            await _context.SaveChangesAsync();

            return "done";
        }


        // PUT: api/Discussions/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult<string>> Update([FromForm] Discussion Discussion)
        {


            _context.Entry(Discussion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Done";
        }
        [HttpGet("custom/{id}")]
        public async Task<ActionResult<Discussion>> GetDiscussions(int id){
          var discussions= await _context.Discussions.Where(m=>m.IdSender==id || m.IdReceiver==id).ToListAsync();
      return Ok(discussions);
            // return Ok(id);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var DiscussionToDelete = await _context.Discussions.FindAsync(id);
            if (DiscussionToDelete == null)
            {
                return NotFound();
            }

            _context.Discussions.Remove(DiscussionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }



}
