using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskItemView : MonoBehaviour
{
    [SerializeField] private Toggle _checkbox;
    [SerializeField] private TextMeshProUGUI _titleText;
    private EventHandler _eventHandler;

    private int _taskId;

    public void Initialize(TaskViewModel taskViewModel, EventHandler eventHandler)
    {
        _eventHandler = eventHandler;

        _taskId = taskViewModel.TaskId;
        _titleText.text = taskViewModel.DisplayTitle;

        _checkbox.onValueChanged.RemoveAllListeners(); // remove old listeners for checkbox
        _checkbox.isOn = taskViewModel.IsCompleted;
        _checkbox.onValueChanged.AddListener((isOn) =>
        {
            OnCheckboxValueChanged(isOn);
        });
    }

    private void OnCheckboxValueChanged(bool isCompleted)
    {
        _eventHandler.HandleTaskCompleted(_taskId);
    }
}