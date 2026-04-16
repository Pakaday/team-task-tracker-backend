using Microsoft.EntityFrameworkCore;
using team_task_tracker_backend.Models;

namespace team_task_tracker_backend.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<TaskItem> TaskItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();

			modelBuilder.Entity<TaskItem>()
				.Property(e => e.Status)
				.HasConversion<string>();
		}
	}
}
