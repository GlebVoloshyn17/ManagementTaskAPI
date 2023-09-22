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
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            if (taskItem == null)
            {
                return BadRequest("Task data is invalid.");
            }

            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Id == taskItem.UserId);
                var taskListExists = await _context.TaskLists.AnyAsync(tl => tl.Id == taskItem.TaskListId);

                if (!userExists)
                {
                    return NotFound("User not found.");
                }

                if (!taskListExists)
                {
                    return NotFound("Task list not found.");
                }

                _context.Tasks.Add(taskItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTaskById", new { id = taskItem.Id }, taskItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET: api/task/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT: api/task/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem updatedTask)
        {
            if (updatedTask == null || id != updatedTask.Id)
            {
                return BadRequest("Task data is invalid.");
            }

            try
            {
                var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

                if (existingTask == null)
                {
                    return NotFound("Task not found.");
                }

                existingTask.Title = updatedTask.Title;
                existingTask.Description = updatedTask.Description;
                existingTask.DueDate = updatedTask.DueDate;
                existingTask.IsCompleted = updatedTask.IsCompleted;

                _context.Tasks.Update(existingTask);
                await _context.SaveChangesAsync();

                return Ok(existingTask);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE: api/task/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
