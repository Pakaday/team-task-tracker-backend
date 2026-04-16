using System.ComponentModel.DataAnnotations;

namespace team_task_tracker_backend.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public required string Name { get; set; }

		public string? Email { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
