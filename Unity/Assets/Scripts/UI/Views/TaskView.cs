using System.Collections.Generic;
using UnityEngine;

public abstract class TaskView : MonoBehaviour
{
    protected EventHandler _eventHandler;

    public virtual void Render(List<TaskViewModel> taskViewModelList)
    {
        
    }

    public virtual void Initialize(EventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }
}
