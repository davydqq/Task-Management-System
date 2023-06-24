using Common.Database;

namespace Common.DTO;

public class TaskCreate
{
    public string TaskName { set; get; }

    public string Description { get; set; }

    public string AssignedTo { get; set; }

    public TaskStatusEnum StatusId { get; set; }

    public TaskEntity NewTask()
    {
        return new TaskEntity
        {
            StatusId = StatusId,
            AssignedTo = AssignedTo,
            Description = Description,
            TaskName = TaskName,
        };
    }
}
