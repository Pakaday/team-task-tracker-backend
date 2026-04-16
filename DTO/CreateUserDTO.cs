using System.ComponentModel.DataAnnotations;

namespace team_task_tracker_backend.DTO
{
	public class CreateUserDTO
	{
		[Required(ErrorMessage = "Name is required")]
		public required string Name { get; set; } = null!;
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string? Email { get; set; }
	}
}
