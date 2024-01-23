using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TODOListProject.Rubens;

namespace TODOListProject.Ef;

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
        var atomIdConverter = new ValueConverter<AtomID, ulong>(
            atomId => atomId.Value,
            ulongValue => new AtomID(ulongValue));


        if (_todoContextOptions.Schema.IsNullOrEmpty())
        {
            modelBuilder.Entity<Todo>()
                .ToTable(_todoContextOptions.TableName);
        }
        else
        {
            modelBuilder.Entity<Todo>()
                .ToTable(_todoContextOptions.TableName, _todoContextOptions.Schema);
        }

        modelBuilder.Entity<Todo>()
            .Property(e => e.AtomId)
            .HasConversion(atomIdConverter)
            .ValueGeneratedNever();

        modelBuilder.Entity<Todo>()
            .Property(b => b.Id)
            .ValueGeneratedNever();
    }
}