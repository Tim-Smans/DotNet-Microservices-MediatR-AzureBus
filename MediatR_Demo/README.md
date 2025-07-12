# Todo App (MediatR practice project)

This is the main monolith API part of my Todo App practice project. The main goal is to practice using MediatR and Clean Architecture principles in a .NET application.
This README mainly focuses on the used tech stack and how these components interact with eachother.

## Technology stack
* .NET 8 Web API
* Entity Framework Core
* MediatR (CQRS + Pipeline)
* AutoMapper
* Clean Architecture
* SQL Server

#### Web API
To build the API routes, I created a .NET 8 Web API.
As a big part of clean architecture, the controller does not contain any business logic, everything is handled by the mediator.

To catch exceptions I am using a custom exception handler, this means I did not have to try/catch any routes in the controllers.

##### Api routes

| Methode | Route                     | Description                        | Request/Response Body                          |
|---------|---------------------------|-------------------------------------|----------------------------------------|
| POST    | `/api/todo`      | Create a new todo task | `{"name": "string","description": "string"}` |
| GET    | `/api/todo`      | Get all todo tasks | `[{ "id": "605876e3-b1d8-4c34-b54e-1787b6d736fc","name": "Auto wassen","description": "Er moet dringend een auto gewassen worden","created": "2025-07-07T20:48:52.2600288","completed": false},]` |
| GET    | `/api/todo/{id}`      | Get a specific todo task | `{ "id": "605876e3-b1d8-4c34-b54e-1787b6d736fc","name": "Auto wassen","description": "Er moet dringend een auto gewassen worden","created": "2025-07-07T20:48:52.2600288","completed": false}` |
| DELETE    | `/api/todo/{id}`      | Delete a todo task | No content |
---

#### Entity Framework Core

For entity framework I ofcourse created the database context.
I also created repositories and a Unit Of Work patern. All of these are implemented using dependency inversion.
Meaning that high-level modules will not depend on low-level modules. In our case we always dependency inject an interface of the repository / Unit Of Work.
The usage of Entity Framework Core is quite basic in this application, since this was not the main purpose.

---

#### MediatR
MediatR is used to abstract the connection between my controllers and services that talk to the data layer. This is also called the CQRS patern.

We split up our usecases into multiple **Commands**, **Queries** and then handlers for each of these. 
This makes our file structure look something like this:
```
Todos/
├── Commands/
│   ├── CreateTodo/
│   │   ├── CreateTodoCommand.cs
│   │   └── CreateTodoHandler.cs
│   └── DeleteTodo/
│       ├── DeleteTodoCommand.cs
│       └── DeleteTodoHandler.cs
└── Queries/
    ├── ReadAllTodos/
    │   ├── ReadAllTodosHandler.cs
    │   └── ReadAllTodosQuery.cs
    └── ReadTodoById/
        ├── ReadTodoByIdHandler.cs
        └── ReadTodoByIdQuery.cs
```

Each usecase has a command or query, and a handler. 
In a **Command** or **Query** we define *Request body*, *Parameters* and/or *Return types*

**Command (Query is basically the same)**
```c#
public record CreateTodoCommand(string Name, string description) : IRequest<TodoDto>;
```
And then in the handler we built the actual service, this is code that will dependency inject our Unit Of Work, AutoMapper, or maybe other services.
Here we will code the business logic for the usecase that we are working on.

**Handler**
```c#
public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, TodoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTodoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TodoDto> Handle(CreateTodoCommand request, CancellationToken ct)
    {
        Todo todo = _mapper.Map<Todo>(request);

        todo.Id = Guid.NewGuid();

        await _unitOfWork.TodoRepo.AddAsync(todo);
        await _unitOfWork.SaveChanges(ct);

        return _mapper.Map<TodoDto>(todo);
    }
}
```
The handler needs to implement `IRequestHandler<CommandOrQuerry, ReturnType>`

For void types we can use MediatR's *Unit* type as a return type. 


##### MediatR Pipeline
MediatR also has the possibility to customize pipelines, this means that we can add steps to our mediator pipelines.
For example we can add custom exception handling:

```C#
try
{
    // Run the actual handler
    return await next();
}
catch (NotFoundException ex)
{
    _logger.LogWarning(ex, "Not found: {Request}", typeof(TRequest).Name);
    throw new NotFoundException(ex.Message);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unhandled exception for request {Request}", typeof(TRequest).Name);
    throw new ApplicationException("Something went wrong");
}
```

---
#### AutoMapper
AutoMapper is used as a tool to simplify our mapping process. We can simply create a Mapper profile where we can configure our mappings, something like this:
```c#
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateTodoCommand, Todo>();
        CreateMap<Todo, TodoDto>();
    }
}
```

Then we can use the IMapper dependency injection to actually map the objects:
```c#
Todo todo = _mapper.Map<Todo>(request);
```

We can also use way more advanced techniques using automapper to create complex mappings, but that is out of scope for this project. More info can be found here: https://docs.automapper.io/en/latest/Conditional-mapping.html

---
#### Clean Architecture

This project is built using the Clean Architecture patern (also called onion architecture), it's a layered architecture, which we can visualize as concentric circles. Hence the name onion architecture. 

The architecture exists out of the following layers:

![image.png](https://i.imgur.com/9FV9gQm.png)

##### **Domain layer:**

This is the core layer of the application, it includes the core logic of the app and is completely independent from external systems.

What is included in a **.NET Project:**

- Domain entities: Todo, User
- Value Objects
- Interfaces of repositories or services (Example: *ITodoRepository*)
- Business rules and validations

The typical project directory name is: **AppName.Domain**

##### Service layer (Also called Application Layer):

This layer controls use cases, this layer knows the domain but does not know the presentation or infrastructure layer.

What is included in a **.NET Project:**

- Use cases / Command handlers (Example: *CreateTodoHandler*)
- Application services 
- AutoMapper config (Or manual mappings)
- Data Transfer Objects

The typical project directory name is: **AppName.Application**

##### **Infrastructure Layer:**

This layer is responsible for external systems, like databases, filesystems, external API’s.

What is included in a **.NET Project:**

- Entity framework implementations (Repositories)
- Data contexts (Example: *AppDbContext*)
- Implementations for loggings, emails or file storages
- Service adapters to external API’s

The typical project directory name is: **AppName.Infrastructure**

##### Project referencing

You can see the references in the image above, a layer of the architecture is able to reference everything inside of their layer.

For example: 
**Infrastructure** and **Presentation** layers are able to reference **Service** and **Domain**
Service layer is able to reference **Domain**
Domain is not able to reference anything.

---
