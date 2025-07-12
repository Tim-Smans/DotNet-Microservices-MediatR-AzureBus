using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Domain.Dto.Todo
{
    public class CreateTodoDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
