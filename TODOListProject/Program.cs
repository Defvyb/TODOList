using TODOListProject;
using TODOListProject.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();


builder.Services.AddSingleton<TodoContextOptions>(provider =>
{
    var configuration = provider.GetService<IConfiguration>();
    if (configuration == null)
    {
        throw new InvalidOperationException("Configuration service is not available.");
    }

    var tableName = configuration["TodoTableName"];
    if (string.IsNullOrEmpty(tableName))
    {
        throw new InvalidOperationException("TodoTableName is not configured.");
    }
    
    var schemaName = configuration["Schema"];
    if (string.IsNullOrEmpty(schemaName))
    {
        throw new InvalidOperationException("Schema is not configured.");
    }

    return new TodoContextOptions
    {
        TableName = tableName,
        Schema = schemaName
    };
});

builder.Services.AddDbContext<TodoContext>((serviceProvider, options) =>
{
    var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});


builder.Services.AddScoped<ITaskDb, EfTaskDb>();

builder.Services.AddSingleton<ITaskList>(sp => new TaskList(sp.GetRequiredService<IServiceScopeFactory>()));


var app = builder.Build();

app.MapGrpcService<TODOService>();
app.MapGrpcReflectionService(); 

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
