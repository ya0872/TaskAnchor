using System.Collections.Generic;

public interface ITaskRepository
{
    public void InitializeDatabase(string databaseName);
    public void CreateTaskTable();
    public void Save(Task task);
    public void Update(Task task);
    public void Delete(int taskId);
    public Task FindTaskById(int taskId);
    public List<Task> FindAllTasks();
}
