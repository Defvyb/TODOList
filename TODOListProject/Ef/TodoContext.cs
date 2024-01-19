using TODOListProject;
using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
    private readonly TodoContextOptions _todoContextOptions;
    public DbSet<Todo> Todos { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options, TodoContextOptions todoContextOptions) : base(options)
    {
        _todoContextOptions = todoContextOptions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .ToTable(_todoContextOptions.TableName, _todoContextOptions.Schema)
            .Property(b => b.Id)
            .ValueGeneratedNever();
    }
}