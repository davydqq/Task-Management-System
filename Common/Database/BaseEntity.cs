
namespace Common.DatabaseModels;

public class BaseEntity<T>
{
    public virtual T Id { set; get; }
}
