using Microsoft.EntityFrameworkCore;
using ToDoApi2.Models;

namespace ToDoApi2.Data
{
    public class ToDoDbContext : DbContext
    {
        // Constructor for the context, accepting options and passing them to the base DbContext class
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { }
        public DbSet<ToDo> ToDos { get; set; }
    }
}
