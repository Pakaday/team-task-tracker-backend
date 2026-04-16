using System.ComponentModel.DataAnnotations;

namespace team_task_tracker_backend.Models
{

	public enum TaskStatus
	{
		New,
		InProgress,
		Completed
	}

	public class TaskItem
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Title is required")]
		public required string Title { get; set; }

		public string? Description { get; set; }
		
		public DateOnly DueDate { get; set; }
		
		public TaskStatus Status { get; set; }
		
		public DateTime CreatedAt { get; set; }
		
		public DateTime UpdatedAt { get; set; }
	}
}
