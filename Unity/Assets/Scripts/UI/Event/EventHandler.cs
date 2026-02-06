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

        // 初期表示：全タスク読み込み
        List<TaskViewModel> taskViewModelList = _taskController.LoadAllTasks();
        _taskListView.Render(taskViewModelList);
    }

    public void HandleTaskCompleted(int taskId)
    {
        // タスク完了処理（内部で削除＆再取得）
        List<TaskViewModel> taskViewModelList = _taskController.CompleteTask(taskId);
        _taskListView.Show(taskViewModelList);
    }

    // 【修正】文字列を受け取って新規作成するメソッドを追加
    public void HandleCreateNewTask(string title)
    {
        // Controllerに追加を依頼
        List<TaskViewModel> taskViewModelList = _taskController.AddTask(title);
        _taskListView.Show(taskViewModelList);
    }
}