using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace openrmf_templates_api.Controllers
{
    [Route("healthz")]
    public class HealthController : Controller
    {
       private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        // GET the listing with Ids of all the scores for the checklists
        [HttpGet]
        public ActionResult<string> Get()
        {
            try {
                _logger.LogInformation(string.Format("/healthz: healthcheck heartbeat"));
                return Ok("ok");
            }
            catch (Exception ex){
                _logger.LogError(ex, "Healthz check failed!");
                return BadRequest("Improper API configuration"); 
            }
        }
    }
}