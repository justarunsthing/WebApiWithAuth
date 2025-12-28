using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiWithAuth.Data;
using WebApiWithAuth.Models;

namespace WebApiWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskerItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Expression-bodied member
        private string? UserId => _userManager.GetUserId(User);

        public TaskerItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/TaskerItems
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskerItem>>> GetTaskerItems()
        {
            return await _context.TaskerItems.ToListAsync();
        }

        // GET: api/TaskerItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskerItem>> GetTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FindAsync(id);

            if (taskerItem == null)
            {
                return NotFound();
            }

            return taskerItem;
        }

        // PUT: api/TaskerItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskerItem(int id, TaskerItem taskerItem)
        {
            if (id != taskerItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskerItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskerItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskerItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskerItem>> PostTaskerItem(TaskerItem taskerItem)
        {
            _context.TaskerItems.Add(taskerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskerItem", new { id = taskerItem.Id }, taskerItem);
        }

        // DELETE: api/TaskerItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FindAsync(id);
            if (taskerItem == null)
            {
                return NotFound();
            }

            _context.TaskerItems.Remove(taskerItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskerItemExists(int id)
        {
            return _context.TaskerItems.Any(e => e.Id == id);
        }
    }
}