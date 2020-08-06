using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PostmanautService.Hubs;

namespace PostmanautService.Controllers
{
    public class Action
    {
        public string perform { get; set; }
        public string direction { get; set; }
    }

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
        public IActionResult SendAction(string contextId, [FromBody] Action action)
        {
            string command = $"{action.perform},{action.direction}";
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
}
