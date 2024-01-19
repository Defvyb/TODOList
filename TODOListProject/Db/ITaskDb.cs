namespace TODOListProject;

public interface ITaskDb
{
    bool Add(int id, string name);
    bool Delete(int id);
    Dictionary<int, string> GetList();
}