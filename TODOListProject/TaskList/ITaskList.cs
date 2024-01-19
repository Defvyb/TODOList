namespace TODOListProject;

public interface ITaskList
{
    bool Add(int id, string name);
    bool Delete(int id);
    List<string> GetList();
}