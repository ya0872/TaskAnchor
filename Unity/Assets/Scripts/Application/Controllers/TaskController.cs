using System.Collections.Generic;

public class TaskController
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskViewModelMapper _taskViewModelMapper;

    /*private List<Task> sampleTasks = new List<Task>
    {
        new Task { TaskId = 1, Title = "牛乳を買う", IsCompleted = false },
        new Task { TaskId = 2, Title = "Unity勉強", IsCompleted = false },
        new Task { TaskId = 3, Title = "部屋の掃除", IsCompleted = false },
    };*/

    public TaskController(ITaskRepository taskRepository, ITaskViewModelMapper taskViewModelMapper)
    {
        _taskRepository = taskRepository;
        _taskViewModelMapper = taskViewModelMapper;

        // Sample data insertion
        /*foreach (var task in sampleTasks)
        {
            _taskRepository.Save(task);
        }*/
    }

    public List<TaskViewModel> CompleteTask(int taskId)
    {
        _taskRepository.Delete(taskId);

        List<Task> taskList = _taskRepository.FindAllTasks();
        List<TaskViewModel> taskViewModelList = new List<TaskViewModel>();
        foreach (var t in taskList)
        {
            taskViewModelList.Add(_taskViewModelMapper.MapTask(t));
        }

        return taskViewModelList;
    }

    public List<TaskViewModel> AddTask(int taskId)
    {
        Task task = _taskRepository.FindTaskById(taskId);
        _taskRepository.Save(task);

        List<Task> taskList = _taskRepository.FindAllTasks();
        List<TaskViewModel> taskViewModelList = new List<TaskViewModel>();
        foreach (var t in taskList)
        {
            taskViewModelList.Add(_taskViewModelMapper.MapTask(t));
        }

        return taskViewModelList;
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
