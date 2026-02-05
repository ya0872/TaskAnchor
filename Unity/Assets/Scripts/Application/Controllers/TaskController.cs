using System.Collections.Generic;

public class TaskController
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskViewModelMapper _taskViewModelMapper;

    public TaskController(ITaskRepository taskRepository, ITaskViewModelMapper taskViewModelMapper)
    {
        _taskRepository = taskRepository;
        _taskViewModelMapper = taskViewModelMapper;
    }

    public void CompleteTask(int taskId)
    {

    }

    public void AddTask(int taskId)
    {

    }

    public void DeleteTask(int taskId)
    {

    }
}
