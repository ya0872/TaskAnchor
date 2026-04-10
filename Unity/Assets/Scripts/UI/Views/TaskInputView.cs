using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskInputView : TaskView
{
    [SerializeField] private TMP_InputField _taskInputField;

    public override void Render(List<TaskViewModel> taskViewModelList)
    {
        
    }

    public void NotifySubmit(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return; // nullバリデーション

        //文字数バリデーション
        if (title.Length > 20) 
        {
            _taskInputField.text = "";
            return;
        } 
        
        _eventHandler.HandleTaskAdded(title);
        _taskInputField.text = "";
    }
}
