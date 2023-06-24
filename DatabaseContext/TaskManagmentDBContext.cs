using Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace DatabaseContext;

public class TaskManagmentDBContext : DbContext
{
    public DbSet<TaskEntity> TaskEntities { set; get; }

    public DbSet<TaskStatusEntity> TaskStatusEntity { set; get; }

    public TaskManagmentDBContext(DbContextOptions<TaskManagmentDBContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskStatusEntity>().HasData(
                new TaskStatusEntity { Id = TaskStatusEnum.NotStarted, Name = nameof(TaskStatusEnum.NotStarted) },
                new TaskStatusEntity { Id = TaskStatusEnum.InProgress, Name = nameof(TaskStatusEnum.InProgress) },
                new TaskStatusEntity { Id = TaskStatusEnum.Completed, Name = nameof(TaskStatusEnum.Completed) });
    }
}
