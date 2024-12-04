using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ToDoApi2.Data;
using ToDoApi2.Models;

// Initialize the WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Configure the database context to use MySQL with the specified version and connection string
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31)) // Specify the MySQL server version
    ));

var app = builder.Build();

// Set up URL rewriting: Redirect root URL ("") to "/todos"
app.UseRewriter(new RewriteOptions().AddRedirect("^$", "/todos"));

// Define a GET endpoint to fetch all ToDo items
app.MapGet("/todos", async (ToDoDbContext db) =>
    await db.ToDos.ToListAsync());

// Define a GET endpoint to fetch a ToDo item by its ID
app.MapGet("/todos/{id}", async (int id, ToDoDbContext db) =>
    await db.ToDos.FindAsync(id) is ToDo todo ? Results.Ok(todo) : Results.NotFound());

// Define a GET endpoint to fetch ToDo items with expiry dates in the next 7 days
app.MapGet("/todos/incoming", async (ToDoDbContext db) =>
    await db.ToDos
        .Where(t => t.ExpiryDate >= DateTime.Now && t.ExpiryDate <= DateTime.Now.AddDays(7))
        .ToListAsync());

// Define a POST endpoint to add a new ToDo item
app.MapPost("/todos", async (ToDo todo, ToDoDbContext db) =>
{
    // Validate the provided ToDo item
    if (!IsValid(todo))
    {
        // Return a bad request with validation errors
        return Results.BadRequest("Invalid data: " + string.Join(", ", GetValidationErrors(todo)));
    }

    // Add the ToDo item to the database and save changes
    db.ToDos.Add(todo);
    await db.SaveChangesAsync();

    // Return a Created response with the location of the new resource
    return Results.Created($"/todos/{todo.Id}", todo);
});

// Define a PUT endpoint to update an existing ToDo item by its ID
app.MapPut("/todos/{id}", async (int id, ToDo updatedToDo, ToDoDbContext db) =>
{
    // Validate the updated ToDo item
    if (!IsValid(updatedToDo))
    {
        // Return a bad request with validation errors
        return Results.BadRequest("Invalid data: " + string.Join(", ", GetValidationErrors(updatedToDo)));
    }

    // Find the existing ToDo item by ID
    var todo = await db.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound(); // Return NotFound if the item does not exist

    // Update the fields of the ToDo item
    todo.Title = updatedToDo.Title;
    todo.Description = updatedToDo.Description;
    todo.CompletionPercentage = updatedToDo.CompletionPercentage;
    todo.IsComplete = updatedToDo.IsComplete;
    todo.ExpiryDate = updatedToDo.ExpiryDate;

    // Save changes to the database
    await db.SaveChangesAsync();
    return Results.NoContent(); // Return NoContent indicating a successful update
});

// Define a POST endpoint to mark a ToDo item as "done" by its ID
app.MapPost("/todos/{id}/done", async (int id, ToDoDbContext db) =>
{
    // Find the ToDo item by ID
    var todo = await db.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound(); // Return NotFound if the item does not exist

    // Mark the item as complete
    todo.IsComplete = true;

    // Save changes to the database
    await db.SaveChangesAsync();
    return Results.Ok(todo); // Return the updated ToDo item
});

// Define a DELETE endpoint to remove a ToDo item by its ID
app.MapDelete("/todos/{id}", async (int id, ToDoDbContext db) =>
{
    // Find the ToDo item by ID
    var todo = await db.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound(); // Return NotFound if the item does not exist

    // Remove the ToDo item from the database and save changes
    db.ToDos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent(); // Return NoContent indicating a successful deletion
});

// Run the application
app.Run();

// Helper method to validate a ToDo item using data annotations
bool IsValid(ToDo todo)
{
    var validationResult = new List<ValidationResult>();
    var context = new ValidationContext(todo);
    return Validator.TryValidateObject(todo, context, validationResult, true);
}

// Helper method to retrieve validation errors for a ToDo item
List<string> GetValidationErrors(ToDo todo)
{
    var validationResult = new List<ValidationResult>();
    var context = new ValidationContext(todo);
    Validator.TryValidateObject(todo, context, validationResult, true);

    // Return the list of error messages
    return validationResult.Select(x => x.ErrorMessage).ToList();
}
