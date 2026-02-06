using System.Collections.Generic;

public class TaskViewModel
{
    // プロパティに変更（頭文字が大文字）
    public int TaskId { get; }
    public string DisplayTitle { get; }
    public bool IsCompleted { get; }

    public TaskViewModel(int taskId, string displayTitle, bool isCompleted)
    {
        TaskId = taskId;
        DisplayTitle = displayTitle;
        IsCompleted = isCompleted;
    }
}