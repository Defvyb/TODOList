using Grpc.Core;
using TODOListProject;

namespace TODOListProject.Services;

public class TODOService : TODO.TODOBase
{
    private readonly ITaskList _itodoList;
    
    private readonly ILogger<TODOService> _logger;

    public TODOService(ILogger<TODOService> logger, ITaskList itodoList)
    {
        _itodoList = itodoList;
        _logger = logger;
    }

    public override Task<ListReply> List(ListRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ListReply
        {
            Tasks = { _itodoList.GetList() }
        });
    }
    public override Task<AddReply> Add(AddRequest request, ServerCallContext context)
    {
        bool result = _itodoList.Add(request.Id, request.Task);
        var resultString = result ? "Added" : "Not Added";
        return Task.FromResult(new AddReply
        {
            Result = resultString
        });
    }
    public override Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
    {
        bool result = _itodoList.Delete(request.Id);
        var resultString = result ? "Deleted" : "Not Deleted";
        return Task.FromResult(new DeleteReply{ Result = resultString });
    }
}