using System.Collections.Generic;
using SQLite4Unity3d;

public class Task
{
    [PrimaryKey, AutoIncrement]
    public int TaskId { get; set;}
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
