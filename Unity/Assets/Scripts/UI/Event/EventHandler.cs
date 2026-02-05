using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private TaskController _taskController;
    private UILayoutFixer _uiLayoutFixer;

    public void Initialize(TaskController taskController, UILayoutFixer uiLayoutFixer)
    {
        _taskController = taskController;
        _uiLayoutFixer = uiLayoutFixer;
    }

    public void HandleTaskCompleted(int taskId)
    {
        
    }

    public void HandleTaskAdded(int taskId)
    {
        
    }
}
