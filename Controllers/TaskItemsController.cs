using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team_task_tracker_backend.Data;
using team_task_tracker_backend.Models;
using team_task_tracker_backend.DTO;

namespace team_task_tracker_backend.Controllers
{
	[ApiController]
	[Route("api/taskitems")]
	public class TaskItemsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public TaskItemsController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
		{
			return await _context.TaskItems.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TaskItem>> GetTaskItemById(int id)
		{
			var taskItem = await _context.TaskItems.FindAsync(id);
			if (taskItem == null)
			{
				return NotFound();
			}
			return taskItem;
		}

		[HttpPost]
		public async Task<ActionResult<ReadTaskItemDTO>> CreateTaskItem([FromBody]CreateTaskItemDTO createTaskItemDTO)
		{

			if (ModelState.IsValid)
			{
				var taskItem = new TaskItem
				{
					Title = createTaskItemDTO.Title,
					Description = createTaskItemDTO.Description,
					DueDate = createTaskItemDTO.DueDate ?? default,
					Status = Enum.TryParse<Models.TaskStatus>(createTaskItemDTO.Status, out var status) ? status : Models.TaskStatus.New,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};

				_context.TaskItems.Add(taskItem);

				await _context.SaveChangesAsync();

				var createdTaskItemDTO = new ReadTaskItemDTO
				{
					Id = taskItem.Id,
					Title = taskItem.Title,
					Description = taskItem.Description,
					DueDate = taskItem.DueDate,
					Status = taskItem.Status.ToString(),
					CreatedAt = taskItem.CreatedAt,
					UpdatedAt = taskItem.UpdatedAt
				};

				return CreatedAtAction(
					nameof(GetTaskItemById),
					new { id = createdTaskItemDTO.Id },
					createdTaskItemDTO
				);
			} else
			{
				return BadRequest(ModelState);
			}
		}
	}
}
