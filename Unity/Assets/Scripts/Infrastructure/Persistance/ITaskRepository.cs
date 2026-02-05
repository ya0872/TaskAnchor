public interface ITaskRepository
{
    public void SaveDB(Task task);
    public void UpdateDB(Task task);
    public void DeleteDB(int taskId);
    public Task FindTaskFromId(int taskId);
}
