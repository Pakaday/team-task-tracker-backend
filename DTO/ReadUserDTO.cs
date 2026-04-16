namespace team_task_tracker_backend.DTO
{
	public class ReadUserDTO
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Email { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
