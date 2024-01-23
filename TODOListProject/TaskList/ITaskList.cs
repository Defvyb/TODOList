using TODOListProject.Rubens;

namespace TODOListProject.TaskList;

public interface ITaskList
{
    Guid? tryAdd(string name);
    bool tryDelete(Guid id);
    Dictionary<string, string> GetList();
    Dictionary<string, AtomID> GetAtomIdList();
}