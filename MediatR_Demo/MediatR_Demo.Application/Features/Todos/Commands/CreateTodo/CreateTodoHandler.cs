using AutoMapper;
using MediatR;
using MediatR_Demo.Domain.Dto.Todo;
using MediatR_Demo.Domain.Entities;
using MediatR_Demo.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Features.Todos.Commands.CreateTodo
{
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
}
