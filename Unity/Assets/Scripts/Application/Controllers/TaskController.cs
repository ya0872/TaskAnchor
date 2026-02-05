using System.Collections.Generic;

public class TaskController
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
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
