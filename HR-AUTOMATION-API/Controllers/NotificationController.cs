using HR_AUTOMATION.Infrastructure.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HR_AUTOMATION_API.Controllers
{
    [Route("notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IHubContext<NotificationHub> _hubContext;


        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Send(string notification)
        {
            await _hubContext.Clients.All.SendAsync("SendNotification",notification);
            return Ok();
        }

    }
}
