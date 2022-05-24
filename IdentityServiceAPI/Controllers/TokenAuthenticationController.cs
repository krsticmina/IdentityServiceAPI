using AutoMapper;
using IdentityServiceAPI.Dtos;
using IdentityServiceBLL.Models;
using IdentityServiceBLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var salt = await service.FindUser(model.UserName);

            model.Password = HashPassword(model.Password, salt);

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

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegistrationDto user) 
        {

            user.Salt = createSalt();

            user.Password = HashPassword(user.Password, user.Salt);

            var employeeToAdd = mapper.Map<UserRegistrationModel>(user);

            var result = await service.RegisterUser(employeeToAdd);

            return CreatedAtAction(nameof(RegisterUser), result);
        }

        private static string HashPassword(string password, string salt)
        { 
            byte[] passbytes = Encoding.Unicode.GetBytes(password);

            byte[] saltbytes = Encoding.Unicode.GetBytes(salt);

            byte[] res = new byte[saltbytes.Length + passbytes.Length];

            for (int i = 0; i < passbytes.Length; i++)
            {
                res[i] = passbytes[i];
            }
            for (int i = 0; i < saltbytes.Length; i++)
            {
                res[passbytes.Length + i] = saltbytes[i];
            }

            byte[] hash = SHA256.HashData(res);

            return Convert.ToBase64String(hash);  
        }

        private static string createSalt() 
        {
            byte[] salt = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(salt);
        }
    }
}