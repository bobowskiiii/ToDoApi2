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
    public class UpdateToDoTests
    {
        private readonly ToDoDbContext _context;

        // Constructor to set up the in-memory database context
        public UpdateToDoTests()
        {
            // Configure the DbContext to use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "ToDoTestDb")
                .Options;

            _context = new ToDoDbContext(options);
            _context.Database.EnsureCreated();
        }

        // Test case to verify updating an existing ToDo item
        [Fact]
        public async Task UpdateToDo_ShouldUpdateExistingToDo()
        {
            // Arrange: Set up initial data
            var newTodo = new ToDo
            {
                Title = "Task to update",
                Description = "This will be updated",
                IsComplete = false,
                ExpiryDate = DateTime.Now.AddDays(1),
                CompletionPercentage = 0
            };

            _context.ToDos.Add(newTodo);
            await _context.SaveChangesAsync();

            // Act: Modify the ToDo item and save changes
            newTodo.Title = "Updated Task";
            _context.ToDos.Update(newTodo);
            await _context.SaveChangesAsync();

            // Assert: Verify the update is persisted in the database
            var updatedTodo = await _context.ToDos.FindAsync(newTodo.Id);
            updatedTodo?.Title.Should().Be("Updated Task");
        }
    }
}
