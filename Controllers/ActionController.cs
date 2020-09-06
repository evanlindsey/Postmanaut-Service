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
        [Route("{contextId}")]
        public async Task<ActionResult<ActionCommand>> SendAction(string contextId, [FromBody] ActionCommand action)
        {
            string command = JsonSerializer.Serialize(action);
            await _hubContext.Clients.Client(contextId).SendAsync("ReceiveAction", command);
            return Ok(action);
        }

        [HttpGet]
        [Route("ls")]
        public ActionResult<ActionList> GetActions()
        {
            var actionList = new ActionList
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

    public class ActionList
    {
        public string[] perform { get; set; }
        public string[] direction { get; set; }
    }
}
