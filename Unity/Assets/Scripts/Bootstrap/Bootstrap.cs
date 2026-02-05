using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EventHandler _eventHandler;
    [SerializeField] private TaskListView _taskListView;
    [SerializeField] private TaskInputView _taskInputView;
    [SerializeField] private UILayoutFixer _uiLayoutFixer;
    private ITaskRepository _taskRepository;
    private ITaskViewModelMapper _taskViewModelMapper;
    private TaskController _taskController;

    void Awake()
    {
        _taskRepository = new TaskDataSource();
        _taskViewModelMapper = new TaskViewModelMapper();

        _taskRepository.InitializeDatabase("TaskDB");
        _taskRepository.CreateTaskTable();

        _taskController = new TaskController(_taskRepository, _taskViewModelMapper);
        _eventHandler.Initialize(_taskController, _uiLayoutFixer, _taskListView, _taskInputView);
        _taskInputView.Initialize(_eventHandler);
        _taskListView.Initialize(_eventHandler);
    }
}