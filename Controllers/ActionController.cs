using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PostmanautService.Hubs;

namespace PostmanautService.Controllers
{
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IHubContext<ActionHub> _hubContext;

        public ActionController(IHubContext<ActionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("{clientId}")]
        public async Task<ActionResult> SendAction(string clientId, [FromBody] dynamic action)
        {
            if (ActionHub.clients.ContainsKey(clientId))
            {
                string command = JsonSerializer.Serialize(action);
                await _hubContext.Clients.Client(clientId).SendAsync("ReceiveAction", command);
                return Ok(action);
            }
            return NotFound();
        }
    }
}
