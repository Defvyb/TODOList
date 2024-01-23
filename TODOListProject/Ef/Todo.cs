using TODOListProject.Rubens;

namespace TODOListProject.Ef;

public class Todo
{
    public string Id { get; set; }  = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AtomID AtomId { get; set; } = new();
}