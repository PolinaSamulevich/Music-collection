using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [EndpointSummary("check if service running")]
    public IActionResult Get()
    {
        return Ok("Auth is alive");
    }
}
