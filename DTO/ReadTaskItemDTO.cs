namespace team_task_tracker_backend.DTO
{
	public class ReadTaskItemDTO
	{
		public int Id { get; set; }
		
		public required string Title { get; set; }
		
		public string? Description { get; set; }
		
		public DateOnly DueDate { get; set; }
		
		public string Status { get; set; } = "New";
		
		public DateTime CreatedAt { get; set; }
		
		public DateTime UpdatedAt { get; set; }
	}
}
