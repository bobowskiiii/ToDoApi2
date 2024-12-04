using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi2.Data;
using ToDoApi2.Models;
using FluentAssertions;

namespace ToDoAPi.UnitTests
{
    public class DeleteToDoTests
    {
        private readonly ToDoDbContext _context;

        // Constructor to initialize the in-memory database
        public DeleteToDoTests()
        {
            // Configure the DbContext to use an in-memory database
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "ToDoTestDb")
                .Options;

            _context = new ToDoDbContext(options);
            _context.Database.EnsureCreated();
        }

        // Test case to verify that a ToDo item is removed successfully
        [Fact]
        public async Task DeleteToDo_ShouldRemoveToDo()
        {
            // Arrange: Create a new ToDo item and save it to the in-memory database
            var newTodo = new ToDo
            {
                Title = "Task to delete",
                Description = "This will be deleted",
                IsComplete = false,
                ExpiryDate = DateTime.Now.AddDays(1),
                CompletionPercentage = 0
            };

            _context.ToDos.Add(newTodo);
            await _context.SaveChangesAsync();

            // Act: Remove the ToDo item and save changes
            _context.ToDos.Remove(newTodo);
            await _context.SaveChangesAsync();

            // Assert: Verify the ToDo item is no longer in the database
            var deletedTodo = await _context.ToDos.FindAsync(newTodo.Id);
            deletedTodo.Should().BeNull();
        }
    }
}
