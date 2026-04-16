using System.ComponentModel.DataAnnotations;

namespace team_task_tracker_backend.DTO
{
	public class UpdateTaskItemDTO
	{
		[Required(ErrorMessage = "Title is required")]
		[StringLength(20, ErrorMessage = "Title cannot be longer than 20 characters.")]
		public required string Title { get; set; }
		
		public string? Description { get; set; }
		
		public DateOnly? DueDate { get; set; }
		
		public string Status { get; set; } = "New";
		
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
