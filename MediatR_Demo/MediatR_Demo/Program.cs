using MediatR;
using MediatR_Demo.Application.Behaviors;
using MediatR_Demo.Application.Features.Todos.Commands.CreateTodo;
using MediatR_Demo.Domain.Exceptions;
using MediatR_Demo.Domain.Interfaces.Repositories;
using MediatR_Demo.Infrastructure;
using MediatR_Demo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register all MediatR Services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTodoHandler>());

// Automapper and entity framework
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDBConnection")));

// Uow service, scoped because it needs to live inside the same HTTP request
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add the exception handling pipeline behavior as a service
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the exception handler for the api responses
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ApplicationException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = exception?.Message
        }));
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
