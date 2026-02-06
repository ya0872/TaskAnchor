using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TaskInputView : TaskView
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button addButton;

    protected override void Start()
    {
        base.Start();
        if (addButton != null) addButton.onClick.AddListener(OnAddButtonClicked);
    }

    private void OnAddButtonClicked()
    {
        if (inputField == null || string.IsNullOrWhiteSpace(inputField.text)) return;

        // EventHandlerに新しいタスク作成を依頼
        if (eventHandler != null)
        {
            eventHandler.HandleCreateNewTask(inputField.text);
        }

        inputField.text = "";
    }

    public override void Render(List<TaskViewModel> taskViewModelList)
    {
        // 入力画面は表示更新なし
    }
}