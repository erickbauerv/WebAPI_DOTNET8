using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            // 1. Validar usuário e senha
            if(userLogin.UserName != "admin" || userLogin.Password != "12345")
                return Unauthorized("Usuário ou senha incorretos");

            // 2. Criar token
            var token = GenerateJwtToken(userLogin.UserName);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string userName)
        {
            // 3. Configurar as informações do token Jwt (Claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Meu Claim Customizado", "ValorCustomizado")
            };

            var jwtConfig = _configuration.GetSection("Jwt");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtConfig["Key"] ?? throw new InvalidOperationException("Nenhuma chave JWT configurada");

            // 4. CRIAR CHAVE SECRETA
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. CONFIGURAR TOKEN
            var token = new JwtSecurityToken(
                    issuer: jwtConfig["Issuer"],
                    audience: jwtConfig["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig["ExpiryInMinutes"])),
                    signingCredentials: credentials
                );

            // 6. GERAR E RETORNAR O TOKEN COMO STRING
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
