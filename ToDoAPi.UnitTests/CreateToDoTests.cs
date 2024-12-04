using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using ToDoApi2.Data;
using ToDoApi2.Models;
namespace ToDoAPi.UnitTests

{
    public class CreateToDoTests
    {
        private readonly ToDoDbContext _context;

        // Constructor to initialize the in-memory database
        public CreateToDoTests()
        {
            // Configure the DbContext to use an in-memory database
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "ToDoTestDb")
                .Options;

            _context = new ToDoDbContext(options);
            _context.Database.EnsureCreated();
        }

        // Test case to verify adding a new ToDo item
        [Fact]
        public async Task CreateToDo_ShouldAddNewToDo()
        {
            // Arrange: Create a new ToDo object with relevant properties
            var newTodo = new ToDo
            {
                Title = "Test Task",
                Description = "Test Description",
                IsComplete = false,
                ExpiryDate = DateTime.Now.AddDays(1),
                CompletionPercentage = 0
            };

            // Act: Add the new ToDo item to the database and save changes
            _context.ToDos.Add(newTodo);
            await _context.SaveChangesAsync();

            // Assert: Verify the new ToDo item is successfully added
            var todoFromDb = await _context.ToDos.FindAsync(newTodo.Id);
            todoFromDb.Should().NotBeNull();
            todoFromDb?.Title.Should().Be("Test Task");
        }
    }
}