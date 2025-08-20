using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace dotnet_api_example.Function
{
    public class CustomJwtBearerHandler : JwtBearerHandler
    {
        public readonly IConfiguration _config;

        public CustomJwtBearerHandler(
           IOptionsMonitor<JwtBearerOptions> options,
           ILoggerFactory logger,
           UrlEncoder encoder,
           ISystemClock clock,
           IConfiguration config
       ) : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 1. AllowAnonymous → lewati autentikasi
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null || endpoint == null)
                return AuthenticateResult.NoResult();

            // 2. Cek Authorization header
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
                return AuthenticateResult.Fail("Authorization header not found.");

            var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return AuthenticateResult.Fail("Bearer token not found in Authorization header.");

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            try
            {
                // 3. Validasi token → dapatkan ClaimsPrincipal
                var principal = GetClaims(token);

                // 4. Return success dengan scheme yang sedang dipakai
                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
                //return AuthenticateResult.Success(new AuthenticationTicket(principal, "CustomJwtBearer"));
            }
            catch (SecurityTokenExpiredException)
            {
                return AuthenticateResult.Fail("Token has expired.");
            }
            catch (SecurityTokenException)
            {
                return AuthenticateResult.Fail("Invalid token.");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Authentication failed.");
                //return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
            }
        }


        private ClaimsPrincipal GetClaims(string token)
        {
            // 🔹 Ambil konfigurasi JWT dari appsettings.json
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey))
                throw new Exception("JWT Key is missing in configuration");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            // 🔹 Parameter validasi token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                RequireExpirationTime = true,
                ValidateLifetime = true,  // token expired ditolak
                ClockSkew = TimeSpan.FromMinutes(5) // toleransi selisih waktu server-client
            };

            var handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);

            return claimsPrincipal;
        }

    }
}
