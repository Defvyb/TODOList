namespace TODOListProject;

public class EfTaskDb : ITaskDb
{
    private readonly TodoContext _context;

    public EfTaskDb(TodoContext context)
    {
        _context = context;
    }

    public bool Add(int id, string name)
    {
        try
        {
            var todo = new Todo { Id = id, Name = name };
            _context.Todos.Add(todo);
            _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Delete(int id)
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

    public Dictionary<int, string> GetList()
    {
        return _context.Todos.ToDictionary(t => t.Id, t => t.Name);
    }
}