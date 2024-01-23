using TODOListProject.Db;
using TODOListProject.Rubens;

namespace TODOListProject.TaskList;

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
    
    public Guid? tryAdd(string name)
    {
        var id = Guid.NewGuid();
        static ulong GetNextId()
        {
            var random = new Random();
            return (ulong)random.Next(1, 1000000);
        }
        var atomId = new AtomID(GetNextId());
        using (var scope = _scopeFactory.CreateScope())
        {
            var taskDb = scope.ServiceProvider.GetRequiredService<ITaskDb>();
            var result =taskDb.Add(id.ToString(), name, atomId);
            if (result)
            {
                _todoList.Add(id.ToString(), name);
                _todoAtomIdList.Add(id.ToString(), atomId);
                return id;
            }
        }

        return null;
    }
    
    public bool tryDelete(Guid id)
    {
        var result = _todoList.Remove(id.ToString());
        if(!result)
            return false;
        using (var scope = _scopeFactory.CreateScope())
        {
            var taskDb = scope.ServiceProvider.GetRequiredService<ITaskDb>();
            return taskDb.Delete(id.ToString());
        }
    }
    
    public Dictionary<string, string> GetList()
    {
        return _todoList;
    }

    public Dictionary<string, AtomID> GetAtomIdList()
    {
        return _todoAtomIdList;
    }
    
    private readonly Dictionary<string, string> _todoList = new();
    private readonly Dictionary<string, AtomID> _todoAtomIdList = new();
}