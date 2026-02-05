using System.Collections.Generic;

public class TaskViewModelMapper : ITaskViewModelMapper
{
    public TaskViewModel MapTask(Task task)
    {
        return new TaskViewModel
        (
            task.TaskId,
            task.Title,
            task.IsCompleted
        );
    }
}
