using AutoMapper;
using MediatR_Demo.Application.Features.Todos.Commands.CreateTodo;
using MediatR_Demo.Domain.Dto.Todo;
using MediatR_Demo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Application.Mapping
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<CreateTodoCommand, Todo>();
            CreateMap<Todo, TodoDto>();
        }

    }
}
