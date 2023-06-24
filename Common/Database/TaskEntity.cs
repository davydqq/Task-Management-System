using Common.DatabaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Database;

public class TaskEntity : BaseEntity<int>
{
    [NotMapped]
    public override int Id { get; set; }

    [Key]
    public int TaskID { get; set; }

    public string TaskName { get; set; }

    public string Description { get; set; }

    public string AssignedTo { get; set; }


    public TaskStatusEnum StatusId { set; get; }
    public TaskStatusEntity Status { set; get; }
}
