using UnityEngine;

/// <summary>
/// 既存のファイルをこの内容で上書きしてください。
/// これでIdやTitleが認識されるようになります。
/// </summary>
public class TaskViewModel
{
    public int Id;
    public string Title;
    public bool IsCompleted;

    public TaskViewModel(int id, string title, bool isCompleted)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
    }
}