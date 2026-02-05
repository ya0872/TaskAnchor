using System.Collections.Generic;

public class TaskViewModelMapper : ITaskViewModelMapper
{
    public TaskViewModel MapTask(Task task)
    {
        return new TaskViewModel
        {
            TaskId = task.TaskId,
            DisplayTitle = task.Title,
            IsCompleted = task.IsCompleted
        };
    }
}
