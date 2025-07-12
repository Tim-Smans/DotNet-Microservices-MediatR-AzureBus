using MediatR;
using MediatR_Demo.Domain.Dto.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Features.Todos.Queries.ReadTodoById
{
    public record ReadTodoByIdQuery(Guid id) : IRequest<TodoDto>;
}
