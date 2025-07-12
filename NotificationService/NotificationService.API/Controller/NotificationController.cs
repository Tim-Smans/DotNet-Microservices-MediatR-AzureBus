using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Interfaces;

namespace NotificationService.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMessageBus _bus;

        public NotificationController(IMessageBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationRequest request)
        {
            await _bus.PublishAsync(request, "notification-queue");
            return Ok("Notification queued!");
        }
    }
    public record NotificationRequest(string Email, string Message);
}
