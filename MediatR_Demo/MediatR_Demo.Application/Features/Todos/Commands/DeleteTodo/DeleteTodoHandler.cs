using AutoMapper;
using MediatR;
using MediatR_Demo.Domain.Entities;
using MediatR_Demo.Domain.Exceptions;
using MediatR_Demo.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Features.Todos.Commands.DeleteTodo
{
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTodoHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken ct)
        {
            Todo? todo = await _unitOfWork.TodoRepo.GetByIdAsync(request.id);

            if (todo == null)
            {
                throw new NotFoundException("No Todo found with this id.");
            }

            _unitOfWork.TodoRepo.Delete(todo);
            await _unitOfWork.SaveChanges(ct);

            return Unit.Value;
        }
    }
}
