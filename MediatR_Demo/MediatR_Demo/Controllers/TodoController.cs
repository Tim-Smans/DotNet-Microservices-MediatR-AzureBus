using MediatR;
using MediatR_Demo.Application.Features.Todos.Commands.CreateTodo;
using MediatR_Demo.Application.Features.Todos.Commands.DeleteTodo;
using MediatR_Demo.Application.Features.Todos.Queries.ReadAllTodos;
using MediatR_Demo.Application.Features.Todos.Queries.ReadTodoById;
using MediatR_Demo.Domain.Dto.Todo;
using Microsoft.AspNetCore.Mvc;

namespace MediatR_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController: ControllerBase
    {

        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoCommand command, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TodoDto createdTodo = await _mediator.Send(command, ct);

            return CreatedAtAction(nameof(ReadTodoById), new { id = createdTodo.Id }, createdTodo);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> ReadAllTodos(CancellationToken ct)
        {

            IEnumerable<TodoDto> todos = await _mediator.Send(new ReadAllTodosQuery(), ct);

            return Ok(todos);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<TodoDto>>> ReadTodoById(Guid id, CancellationToken ct)
        {

            TodoDto todos = await _mediator.Send(new ReadTodoByIdQuery(id), ct);

            return Ok(todos);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteTodoCommand(id), ct);
            return NoContent();
        }
    }
}
