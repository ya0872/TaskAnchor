using System.Collections.Generic;
using UnityEngine;

public class TaskListView : TaskView
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _taskItemPrefab;

    public override void Render(List<TaskViewModel> taskViewModelList) // drow the list of taskViewModel
    {
        foreach (var taskViewModel in taskViewModelList)
        {
            GameObject taskItemObject = GameObject.Instantiate(_taskItemPrefab, _contentParent);
            TaskItemView taskItemView = taskItemObject.GetComponent<TaskItemView>();
            taskItemView.Initialize(taskViewModel, _eventHandler);
        }
    }

    private void Refresh(List<TaskViewModel> taskViewModelList) // refresh the view of taskList
    {
        foreach (Transform child in _contentParent)
        {
            Destroy(child.gameObject); // clear current view
        }

        Render(taskViewModelList);
    }

    public void Show(List<TaskViewModel> taskViewModelList)
    {
        BeforeRender();
        Refresh(taskViewModelList);
        AfterRender();
    }

    private void BeforeRender()
    {
        
    }

    private void AfterRender()
    {
        
    }
}
