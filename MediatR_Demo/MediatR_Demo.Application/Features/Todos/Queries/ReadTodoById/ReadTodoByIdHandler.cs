using AutoMapper;
using MediatR;
using MediatR_Demo.Domain.Dto.Todo;
using MediatR_Demo.Domain.Entities;
using MediatR_Demo.Domain.Exceptions;
using MediatR_Demo.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Features.Todos.Queries.ReadTodoById
{
    public class ReadTodoByIdHandler : IRequestHandler<ReadTodoByIdQuery, TodoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadTodoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TodoDto> Handle(ReadTodoByIdQuery request, CancellationToken cancellationToken)
        {
            Todo? todo = await _unitOfWork.TodoRepo.GetByIdAsync(request.id);

            if (todo == null)
            {
                throw new NotFoundException("Could not find todo with this id.");
            }
            
            return _mapper.Map<TodoDto>(todo);
        }
    }
}
