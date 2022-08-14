using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTNet6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> logger;
        private readonly IConfiguration configuration;
        public AuthorizationController(ILogger<AuthorizationController> logger
            , IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserInfo request)
        {

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", request.userId),
                        new Claim("DisplayName", request.name),
                        new Claim("UserName", request.name)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            //no match 
            //return BadRequest("Invalid credentials");

            //other
            //return BadRequest();
        }

        public class UserInfo
        {
            public string userId { get; set; }
            public string name { get; set; }
            public string password { get; set; }
        }

        public class AuthResponse
        {
            public string authorization { get; set; }
            public int expired { get; set; }
        }
    }



    //var claims = new[] {
    //                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
    //                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
    //                    new Claim("UserId", user.UserId.ToString()),
    //                    new Claim("DisplayName", user.DisplayName),
    //                    new Claim("UserName", user.UserName),
    //                    new Claim("Email", user.Email)
    //                };
}
