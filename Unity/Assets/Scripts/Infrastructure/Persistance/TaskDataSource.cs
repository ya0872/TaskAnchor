using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;

public class TaskDataSource : ITaskRepository
{
    private SQLiteConnection _taskDatabase;

    public void InitializeDatabase(string databaseName)
    {
        string dbPath = System.IO.Path.Combine(Application.persistentDataPath, databaseName);

        // デバッグ用：パスの現在地
        Debug.Log($"DB Path: {dbPath}");

        // 接続を開く
        _taskDatabase = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create); 
    }

    public void CreateTaskTable()
    {
        _taskDatabase.CreateTable<Task>();
    }

    public void Save(Task task)
    {
        if (_taskDatabase == null)
        {
            Debug.LogError("Database not initialized.");
            return;
        }
        _taskDatabase.Insert(task);
    }

    public void Update(Task task)
    {
        if (_taskDatabase == null)
        {
            Debug.LogError("Database not initialized.");
            return;
        }
    }

    public void Delete(int taskId)
    {
        if (_taskDatabase == null)
        {
            Debug.LogError("Database not initialized.");
            return;
        }
        _taskDatabase.Delete<Task>(taskId);
    }

    public Task FindTaskById(int taskId)
    {
        return _taskDatabase.Table<Task>()
                        .Where(t => t.TaskId == taskId)
                        .FirstOrDefault();
    }

    public List<Task> FindAllTasks()
    {
        return _taskDatabase.Table<Task>().ToList();
    }
}
