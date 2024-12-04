# ToDoApi - README
ToDoApi2 is a RESTful API built with ASP.NET Core for managing to-do tasks. The project includes functionality for creating, reading, updating, and deleting (CRUD) tasks, making it a versatile backend solution for task management applications.

Key Features:
- CRUD Operations: Endpoints for managing to-do items, including fields like title, description, completion status, expiration date, and completion percentage.
  
Database Integration:
- Uses MySQL for persistent data storage in production.
- Uses Entity Framework Core with an In-Memory Database for development and testing purposes.
  
Testing:
- Unit and Integration Tests: Written with xUnit and FluentAssertions to ensure high code quality and reliability.
Performance Tests:
- Benchmarked using BenchmarkDotNet to analyze the performance of database operations.
Postman Testing:
- All API endpoints have been tested for correctness and reliability using Postman.
  
API Endpoints:
1.GET /api/todos - Retrieve all to-do items.
2.GET /api/todos/{id} - Retrieve a specific to-do item by ID.
3.GET /todos/incoming: Retrieve ToDo items with expiry dates in the next days.
4.POST /api/todos - Create a new to-do item.
5.POST /todos/{id}/done: Mark a specific ToDo as completed.
6.PUT /api/todos/{id} - Update an existing to-do item by ID.
7.PUT /todos/{id}/percent: Update the completion percentage of a specific ToDo item.
8.DELETE /api/todos/{id} - Delete a to-do item by ID.
  
Technologies Used:
Framework: ASP.NET Core 6+
Database: Entity Framework Core (In-Memory Database)
Testing Tools: xUnit, FluentAssertions, Postman, BenchmarkDotNet

Running the Application:
- Configure the connection string in appsettings.json.
- Restore dependencies using dotnet restore.
- Run the application using dotnet run.
