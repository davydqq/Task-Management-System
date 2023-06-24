using Common.Database;

namespace Common.DTO;

public class TaskUpdate
{
    public int TaskID { get; set; }

    public TaskStatusEnum NewStatus { get; set; }

    public string UpdatedBy { get; set; }
}
