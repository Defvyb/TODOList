using TODOListProject.Db;
using TODOListProject.Rubens;

namespace TODOListProject.Ef;

public class EfTaskDb : ITaskDb
{
    private readonly TodoContext _context;

    public EfTaskDb(TodoContext context)
    {
        _context = context;
    }

    public bool Add(string id, string name, AtomID atomId)
    {
        try
        {
            var todo = new Todo { Id = id, Name = name, AtomId = atomId};
            _context.Todos.Add(todo);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Delete(string id)
    {
        try
        {
            var todo = _context.Todos.Find(id);
            if (todo == null) return false;
            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Dictionary<string, string> GetList()
    {
        return _context.Todos.ToDictionary(t => t.Id, t => t.Name);
    }
    
    public Dictionary<string, AtomID> GetAtomIdList()
    {
        return _context.Todos.ToDictionary(t => t.Id, t => t.AtomId);
    }
}