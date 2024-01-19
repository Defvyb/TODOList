namespace TODOListProject;

public class TaskList: ITaskList
{
    private readonly IServiceScopeFactory _scopeFactory;

    public TaskList(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        using (var scope = _scopeFactory.CreateScope())
        {
            var taskDb = scope.ServiceProvider.GetRequiredService<ITaskDb>();
            _todoList = taskDb.GetList();
        }
    }

    public bool Add(int id, string name)
    {
        if (_todoList.ContainsKey(id))
            return false;
        _todoList.Add(id, name);
        using (var scope = _scopeFactory.CreateScope())
        {
            var taskDb = scope.ServiceProvider.GetRequiredService<ITaskDb>();
            return taskDb.Add( id, name);
        }
    }
    
    public bool Delete(int id)
    {
        var result = _todoList.Remove(id);
        if(!result)
            return false;
        using (var scope = _scopeFactory.CreateScope())
        {
            var taskDb = scope.ServiceProvider.GetRequiredService<ITaskDb>();
            return taskDb.Delete(id);
        }
    }
    
    public List<string> GetList()
    {
        return _todoList.Values.ToList();
    }
    
    private readonly Dictionary<int, string> _todoList = new();
}