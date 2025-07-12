using MediatR;
using MediatR_Demo.Domain.Dto.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Features.Todos.Commands.DeleteTodo
{
    public record DeleteTodoCommand(Guid id) : IRequest<Unit>;

}
