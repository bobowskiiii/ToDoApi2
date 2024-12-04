using Microsoft.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using ToDoApi2.Data;
using ToDoApi2.Models;
using ToDoApi2;

namespace ToDoApi2.PerformanceTests
{
    public class ToDoPerformanceTests
    {
        private ToDoDbContext _context;

        // Setup and initialization before tests
        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "PerformanceTestDb")
                .Options;

            _context = new ToDoDbContext(options);
            _context.Database.EnsureCreated();
        }

        // Performance test for adding ToDo
        [Benchmark]
        public void AddToDo()
        {
            var newTodo = new ToDo
            {
                Title = "Test Task",
                Description = "Test Description",
                IsComplete = false,
                ExpiryDate = DateTime.Now.AddDays(1),
                CompletionPercentage = 0
            };

            _context.ToDos.Add(newTodo);
            _context.SaveChanges();
        }

        // Performance test for fetching all ToDos
        [Benchmark]
        public void GetAllToDos()
        {
            var todos = _context.ToDos.ToList();
        }

        // Performance test for deleting a ToDo
        [Benchmark]
        public void DeleteToDo()
        {
            var todoToDelete = _context.ToDos.FirstOrDefault();
            if (todoToDelete != null)
            {
                _context.ToDos.Remove(todoToDelete);
                _context.SaveChanges();
            }
        }

        // Run all benchmarks
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ToDoPerformanceTests>();
        }
    }
}
