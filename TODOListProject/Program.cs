using Google.Cloud.EntityFrameworkCore.Spanner.Extensions;
using TODOListProject.Services;
using Microsoft.EntityFrameworkCore;
using TODOListProject.Db;
using TODOListProject.Ef;
using TODOListProject.TaskList;

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
    
   

    var databaseProvider = configuration["DatabaseProvider"];

    string schemaName = String.Empty;
    switch (databaseProvider)
    {
        case "Postgres":
            var schemaFromConfig = configuration["Schema"];
            if (string.IsNullOrEmpty(schemaFromConfig))
            {
                throw new InvalidOperationException("Schema is not configured for postgres.");
            }

            schemaName = schemaFromConfig;
            break;
    }

    return new TodoContextOptions
    {
        TableName = tableName,
        Schema = schemaName
    };
});

builder.Services.AddDbContext<TodoContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var databaseProvider = configuration["DatabaseProvider"];

    switch (databaseProvider)
    {
        case "Spanner":
            var spannerConnectionString = configuration.GetConnectionString("SpannerConnection");
            if (string.IsNullOrEmpty(spannerConnectionString))
            {
                throw new InvalidOperationException("ConnectionString is not configured SpannerConnection.");
            }
            options.UseSpanner(spannerConnectionString);
            break;

        case "Postgres":
            var postgresConnectionString = configuration.GetConnectionString("PostgresConnection");
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new InvalidOperationException("ConnectionString is not configured PostgresConnection.");
            }
            options.UseNpgsql(postgresConnectionString);
            break;

        default:
            throw new InvalidOperationException("Unsupported database provider");
    }
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
