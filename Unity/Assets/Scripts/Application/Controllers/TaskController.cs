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

    public List<TaskViewModel> CompleteTask(int taskId)
    {
        _taskRepository.DeleteDB(taskId);

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
        Task task = _taskRepository.FindTaskFromId(taskId);
        _taskRepository.SaveDB(task);

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
