using TODOListProject.Rubens;

namespace TODOListProject.Db;

public interface ITaskDb
{
    bool Add(string id, string name, AtomID atomId);
    bool Delete(string id);
    Dictionary<string, string> GetList();
    Dictionary<string, AtomID> GetAtomIdList();
}