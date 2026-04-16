using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using team_task_tracker_backend.Data;
using team_task_tracker_backend.Models;
using team_task_tracker_backend.DTO;

namespace team_task_tracker_backend.Controllers
{
	[ApiController]
	[Route("api/users")]
	public class UsersController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UsersController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<User>> GetUserById(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return user;
		}

		[HttpPost]
		public async Task<ActionResult<ReadUserDTO>> CreateUser([FromBody] CreateUserDTO createUserDTO)
		{
			if (await _context.Users.AnyAsync(u => u.Email == createUserDTO.Email))
			{
				return Conflict(new { message = "A user with that email already exists." });
			}

			if (ModelState.IsValid)
			{
				var user = new User
				{
					Name = createUserDTO.Name,
					Email = createUserDTO.Email,
					CreatedAt = DateTime.UtcNow
				};

				_context.Users.Add(user);

				await _context.SaveChangesAsync();

				var createdUserDTO = new ReadUserDTO
				{
					Id = user.Id,
					Name = user.Name,
					Email = user.Email,
					CreatedAt = user.CreatedAt
				};

				return CreatedAtAction(
					nameof(GetUserById),
					new { id = createdUserDTO.Id },
					createdUserDTO
				);
			}
			else
			{
				return BadRequest(ModelState);
			}
		}
	}
}
