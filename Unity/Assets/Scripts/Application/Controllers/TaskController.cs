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

    // 完了処理（削除して最新リストを返す）
    public List<TaskViewModel> CompleteTask(int taskId)
    {
        _taskRepository.Delete(taskId);
        return LoadAllTasks();
    }

    // 【修正】タイトルを受け取って新規作成する
    public List<TaskViewModel> AddTask(string title)
    {
        // 新しいタスクを作成 (IDはAutoIncrementなので0でOK)
        Task newTask = new Task
        {
            Title = title,
            IsCompleted = false
        };

        _taskRepository.Save(newTask);
        return LoadAllTasks();
    }

    public List<TaskViewModel> LoadAllTasks()
    {
        List<Task> taskList = _taskRepository.FindAllTasks();
        List<TaskViewModel> taskViewModelList = new List<TaskViewModel>();
        foreach (var t in taskList)
        {
            taskViewModelList.Add(_taskViewModelMapper.MapTask(t));
        }
        return taskViewModelList;
    }
}