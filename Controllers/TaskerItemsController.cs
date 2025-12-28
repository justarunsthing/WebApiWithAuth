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
using WebApiWithAuth.Extensions;
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
        public async Task<ActionResult<IEnumerable<TaskerItemDto>>> GetTaskerItems()
        {
            return await _context.TaskerItems
                                 .Where(t => t.UserId == UserId)
                                 .Select(t => t.ToDTO())
                                 .ToListAsync();
        }

        // GET: api/TaskerItems/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskerItemDto>> GetTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

            if (taskerItem == null)
            {
                return NotFound();
            }

            return taskerItem.ToDTO();
        }

        // PUT: api/TaskerItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskerItem(int id, TaskerItemDto taskerItemDto)
        {
            if (id != taskerItemDto.Id)
            {
                return BadRequest();
            }

            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

            if (taskerItem == null)
            {
                return NotFound();
            }
            else
            {
                taskerItem.Name = taskerItemDto.Name;
                taskerItem.IsComplete = taskerItemDto.IsComplete;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskerItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TaskerItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskerItem>> PostTaskerItem(TaskerItemDto taskerItemDto)
        {
            var taskerItem = new TaskerItem
            {
                Name = taskerItemDto.Name,
                IsComplete = taskerItemDto.IsComplete,
                UserId = UserId
            };

            _context.TaskerItems.Add(taskerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskerItem", new { id = taskerItem.Id }, taskerItem);
        }

        // DELETE: api/TaskerItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskerItem(int id)
        {
            var taskerItem = await _context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);

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