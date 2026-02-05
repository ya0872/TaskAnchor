using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private TaskController _taskController;
    private UILayoutFixer _uiLayoutFixer;
    private TaskListView _taskListView;
    private TaskInputView _taskInputView;

    public void Initialize(TaskController taskController, UILayoutFixer uiLayoutFixer, TaskListView taskListView, TaskInputView taskInputView)
    {
        _taskController = taskController;
        _uiLayoutFixer = uiLayoutFixer;
        _taskListView = taskListView;
        _taskInputView = taskInputView;

        List<TaskViewModel> taskViewModelList =  _taskController.LoadAllTasks();
        _taskListView.Render(taskViewModelList);
        _taskInputView.Render(taskViewModelList);
    }

    public void HandleTaskCompleted(int taskId)
    {
        List<TaskViewModel> taskViewModelList = _taskController.CompleteTask(taskId);
        
        _taskListView.Show(taskViewModelList);
    }

    public void HandleTaskAdded(int taskId)
    {
        List<TaskViewModel> taskViewModelList = _taskController.AddTask(taskId);
        
        _taskListView.Show(taskViewModelList);
    }
}
