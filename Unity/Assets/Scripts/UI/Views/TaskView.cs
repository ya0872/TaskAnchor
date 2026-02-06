using UnityEngine;
using System.Collections.Generic;

public abstract class TaskView : MonoBehaviour
{
    // 具体的な型に変更
    protected EventHandler eventHandler;

    public void Initialize(EventHandler handler)
    {
        this.eventHandler = handler;
    }

    public void Show(List<TaskViewModel> dataList)
    {
        BeforeRender();
        Render(dataList);
        AfterRender();
    }

    protected virtual void BeforeRender() { }
    protected virtual void AfterRender() { }

    // リストを受け取る形に変更
    public abstract void Render(List<TaskViewModel> taskViewModelList);

    protected virtual void Start() { }
}