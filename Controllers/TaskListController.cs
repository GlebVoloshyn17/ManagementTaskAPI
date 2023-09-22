using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ManagementTaskAPI.Models;

namespace ManagementTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskListController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/TaskList
        [HttpPost]
        public async Task<IActionResult> CreateTaskList([FromBody] TaskList taskList)
        {
            if (taskList == null)
            {
                return BadRequest("Task list data is invalid.");
            }

            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == taskList.UserId);

                if (!userExists)
                {
                    return NotFound("User not found.");
                }

                _context.TaskLists.Add(taskList);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTaskListById", new { id = taskList.Id }, taskList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/TaskList/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskListById(Guid id)
        {
            try
            {
                var taskList = await _context.TaskLists.FirstOrDefaultAsync(tl => tl.Id == id);

                if (taskList == null)
                {
                    return NotFound("Task list not found.");
                }

                return Ok(taskList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/TaskList/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskList(Guid id, [FromBody] TaskList updatedTaskList)
        {
            if (updatedTaskList == null || id != updatedTaskList.Id)
            {
                return BadRequest("Task list data is invalid.");
            }

            try
            {
                var existingTaskList = await _context.TaskLists.FirstOrDefaultAsync(tl => tl.Id == id);

                if (existingTaskList == null)
                {
                    return NotFound("Task list not found.");
                }

                existingTaskList.Name = updatedTaskList.Name;
                existingTaskList.Owner = updatedTaskList.Owner;

                _context.TaskLists.Update(existingTaskList);
                await _context.SaveChangesAsync();

                return Ok(existingTaskList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/TaskList/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(Guid id)
        {
            try
            {
                var taskList = await _context.TaskLists.FirstOrDefaultAsync(tl => tl.Id == id);

                if (taskList == null)
                {
                    return NotFound("Task list not found.");
                }

                _context.TaskLists.Remove(taskList);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/TaskList/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTaskListsForUser(Guid userId)
        {
            try
            {
                var taskLists = await _context.TaskLists.Where(tl => tl.UserId == userId).ToListAsync();
                return Ok(taskLists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
