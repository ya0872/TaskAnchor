using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EventHandler _eventHandler;
    [SerializeField] private TaskListView _taskListView;
    [SerializeField] private TaskInputView _taskInputView;
    [SerializeField] private UILayoutFixer _uiLayoutFixer;
    public ITaskViewModelMapper TaskViewModelMapper { get; } = new TaskViewModelMapper();
    public ITaskRepository TaskRepository { get; } = new TaskDataSource();
    public TaskController TaskController { get; private set; }

    void Awake()
    {
        TaskController = new TaskController(TaskRepository, TaskViewModelMapper);
        _eventHandler.Initialize(TaskController, _uiLayoutFixer, _taskListView, _taskInputView);
        _taskInputView.Initialize(_eventHandler);
        _taskListView.Initialize(_eventHandler);
    }
}