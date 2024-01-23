using Grpc.Core;
using TODOListProject.TaskList;

namespace TODOListProject.Services;

public class TODOService : TODO.TODOBase
{
    private readonly ITaskList _iTodoList;
    
    private readonly ILogger<TODOService> _logger;

    public TODOService(ILogger<TODOService> logger, ITaskList iTodoList)
    {
        _iTodoList = iTodoList;
        _logger = logger;
    }

    public override Task<ListReply> List(ListRequest request, ServerCallContext context)
    {
        var taskDictionary = _iTodoList.GetList();
        var convertedTasks = taskDictionary.ToDictionary(k => k.Key.ToString(), v => v.Value);

        var reply = new ListReply();
        foreach (var task in convertedTasks)
        {
            reply.Tasks.Add(task.Key, task.Value);
        }

        return Task.FromResult(reply);
    }
    
    public override Task<ListAtomReply> ListAtom(ListRequest request, ServerCallContext context)
    {
        var taskDictionary = _iTodoList.GetAtomIdList();
        var convertedTasks = taskDictionary.ToDictionary(k => k.Key.ToString(), v => v.Value.Value);

        var reply = new ListAtomReply();
        foreach (var task in convertedTasks)
        {
            reply.Tasks.Add(task.Key, task.Value);
        }

        return Task.FromResult(reply);
    }
    
    private bool TryParseGuid(string guidString, out Guid guid)
    {
        try
        {
            guid = Guid.Parse(guidString);
            return true;
        }
        catch (FormatException)
        {
            _logger.LogError($"Invalid GUID format: {guidString}");
            guid = Guid.Empty;
            return false;
        }
    }
    public override Task<AddReply> Add(AddRequest request, ServerCallContext context)
    {
        Guid? result = _iTodoList.tryAdd(request.Task);
        var resultString = result != null ? "Added" : "Not Added";
        return Task.FromResult(new AddReply { Result = resultString, Id = result?.ToString() ?? string.Empty  });
    }

    public override Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
    {
        if (!TryParseGuid(request.Id, out var taskId))
        {
            return Task.FromResult(new DeleteReply { Result = "Invalid ID format" });
        }

        bool result = _iTodoList.tryDelete(taskId);
        var resultString = result ? "Deleted" : "Not Deleted";
        return Task.FromResult(new DeleteReply { Result = resultString });
    }
}