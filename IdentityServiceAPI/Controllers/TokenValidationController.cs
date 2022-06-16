using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Text;

namespace IdentityServiceAPI.Controllers
{
    [Route("api/validation")]
    [ApiController]
    public class TokenValidationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public TokenValidationController(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ValidateJWT()
        {
            var token = string.Empty;

            var header = Request.Headers["Authorization"];

            if (header.Count == 0) return BadRequest();

            string[] tokenValue = Convert.ToString(header).Trim().Split(" ");

            if (tokenValue.Length > 1) token = tokenValue[1];

            else return BadRequest();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer=true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["Authentication:Audience"],
                ValidIssuer=configuration["Authentication:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = new JwtSecurityToken();

            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch (Exception) 
            {
                return BadRequest();
            }

            return Ok(securityToken);

        }

    }
}
