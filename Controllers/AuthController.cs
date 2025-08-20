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

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            // ✅ 1. Validasi user (contoh manual, bisa pakai DB)
            if (request.Email == "admin@example.com" && request.Password == "123456")
            {
                var user = new User
                {
                    Email = request.Email,
                    Name = "Admin Example"
                };

                // ✅ 2. Generate token
                var token = _jwtService.GenerateToken(user);

                // ✅ 3. Return ke client
                return Ok(new
                {
                    access_token = token,
                    token_type = "Bearer",
                    expires_in = 3600
                });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
