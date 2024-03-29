
using api.Modules.TodoListModule.Domain.Models;
namespace api.Modules.TodoListModule;
public class TodoListContext : DbContext
{    
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoListItem> TodoListItems { get; set; }
    public DbSet<Dummy> Dummies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>().ToTable("TodoList");
        modelBuilder.Entity<TodoListItem>().ToTable("TodoListItem");
        modelBuilder.Entity<TodoList>()
       .HasMany<TodoListItem>(l => l.Items)
       .WithOne(i => i.TodoList)
       .OnDelete(DeleteBehavior.Cascade);
    }
    public TodoListContext(DbContextOptions<TodoListContext> options) : base(options) { }
}
