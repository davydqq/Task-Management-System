using Common.Database;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using DatabaseContext.GenericRepositories;
using TaskManagement.Interfaces;

namespace TaskManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IRepository<TaskEntity, int> taskRepository;
    private readonly IServiceBusHandler serviceBusHandler;

    public TasksController(IRepository<TaskEntity, int> taskRepository, IServiceBusHandler serviceBusHandler)
    {
        this.taskRepository = taskRepository;
        this.serviceBusHandler = serviceBusHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AddTaskAsync([FromBody] TaskCreate taskCreate)
    {
        var ent = taskCreate.NewTask();

        await taskRepository.AddAsync(ent);

        return Ok();
    }

    [HttpPut("{taskId}/status")]
    public  IActionResult UpdateTaskStatus(int taskId, [FromBody] TaskUpdate taskUpdate)
    {
        taskUpdate.TaskID = taskId;
        serviceBusHandler.SendMessage(taskUpdate);

        return Ok();
    }

    [HttpGet]
    public async Task<List<TaskEntity>> GetTasksAsync()
    {
        // Retrieve the list of tasks from the database using the task repository

        // Return the list of tasks
        return await taskRepository.GetAllAsync();
    }
}
