using dotnet_api_example.Models;
using dotnet_api_example.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_api_example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            // contoh validasi simpel
            if (model.Username == "admin" && model.Password == "1234")
            {
                var token = _jwtService.GenerateToken(model.Username);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
