using Common.DatabaseModels;

namespace Common.Database;

public class TaskStatusEntity : BaseEntity<TaskStatusEnum>
{
    public string Name { set; get; }

    public List<TaskEntity> TaskEntities { set; get; }
}
