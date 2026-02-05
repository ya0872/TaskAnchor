using System.Collections.Generic;
using UnityEngine;

public abstract class TaskView : MonoBehaviour
{
    protected EventHandler _eventHandler;

    public virtual void Render(TaskViewModel viewModel)
    {
        
    }

    public virtual void Refresh()
    {

    }

    public virtual void Initialize(EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public void Show()
    {

    }

    protected void BeforeRender()
    {

    }

    protected void AfterRender()
    {

    }
}
