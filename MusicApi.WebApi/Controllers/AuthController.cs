using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;
using MusicApi.Services.Models;

namespace MusicApi.WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        [EndpointSummary("Register a new user")]
        public IActionResult Register(RegisterRequest request)
        {
            var user = _auth.Register(request);
            if (user == null)
                return Conflict("user with this email already exists");
            return Created($"/users/{user.Id}", user);
        }

        [HttpPost("login")]
        [EndpointSummary("log in and get a token")]
        public IActionResult Login(LoginRequest request)
        {
            if (!_auth.Login(request))
                return Problem("invalid email or password", statusCode: StatusCodes.Status401Unauthorized);
            return Ok("login ok");
        }
    }
}
