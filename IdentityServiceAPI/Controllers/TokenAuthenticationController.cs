using AutoMapper;
using IdentityServiceAPI.Models;
using IdentityServiceBLL.Models;
using IdentityServiceBLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServiceAPI.Controllers
{

    [Route("api/jwt")]
    [ApiController]
    public class TokenAuthenticationController : ControllerBase
    {
        private readonly IUserService service;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public TokenAuthenticationController(IUserService service, IConfiguration configuration, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            model.Password = HashPassword(model.Password);
            var user = await service.ValidateUserCredentials(mapper.Map<UserLoginModel>(model));

            var userForClaims = mapper.Map<UserDto>(user);

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration["JwtAuthentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim(ClaimTypes.NameIdentifier, userForClaims.Id.ToString()));
            claimsForToken.Add(new Claim(ClaimTypes.Name, userForClaims.Name));
            claimsForToken.Add(new Claim(ClaimTypes.Role, userForClaims.Role));
            claimsForToken.Add(new Claim(ClaimTypes.Email, userForClaims.Email));
            claimsForToken.Add(new Claim("Username", userForClaims.UserName));

            var jwtSecurityToken = new JwtSecurityToken(
                configuration["JwtAuthentication:Issuer"],
                configuration["JwtAuthentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(15),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private static string HashPassword(string password) 
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            password = Convert.ToBase64String(encode);
            return password;

        }
    }
}