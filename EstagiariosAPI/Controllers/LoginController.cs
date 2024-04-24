using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstagiariosAPI.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginController(DatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        private string CreateAuthenticationToken(Usuario userInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenSecretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtSettings:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtSettings:Issuer"])
                }),

                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenSecretKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Login body)
        {
            var user = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == body.Email);

            if (user == null)
            {
                return NotFound(new Message("Email ou senha incorreto."));
            }
            if (user.Senha != body.Senha)
            {
                return Unauthorized(new Message("Email ou senha incorreto."));
            }

            string token = CreateAuthenticationToken(user);

            return Ok(new LoginSuccess
            {
                Id = user.Id,
                Nome = user.Nome,
                Token = token
            });
        }
    }
}
