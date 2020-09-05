using System.Text.Json;
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
        [Route("{contextId}")]
        public IActionResult SendAction(string contextId, [FromBody] ActionCommand action)
        {
            string command = JsonSerializer.Serialize(action);
            _hubContext.Clients.Client(contextId).SendAsync("ReceiveAction", command);
            return Ok(action);
        }

        [HttpGet]
        [Route("ls")]
        public IActionResult GetActions()
        {
            var actionList = new
            {
                perform = new string[] { "move", "flip", "stop" },
                direction = new string[] { "forward", "back", "left", "right" }
            };
            return Ok(actionList);
        }
    }

    public class ActionCommand
    {
        public string user { get; set; }
        public string perform { get; set; }
        public string direction { get; set; }
    }
}
