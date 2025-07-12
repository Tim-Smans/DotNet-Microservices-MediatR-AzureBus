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

namespace MediatR_Demo.Application.Features.Todos.Queries.ReadAllTodos
{
    public class ReadAllTodosHandler : IRequestHandler<ReadAllTodosQuery, IEnumerable<TodoDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadAllTodosHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> Handle(ReadAllTodosQuery request, CancellationToken ct)
        {
            IEnumerable<Todo> todos = await _unitOfWork.TodoRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoDto>>(todos);
        }
    }
}
