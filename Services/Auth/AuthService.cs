using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_DOTNET8.Data;
using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Auth
{
    public class AuthService : IAuthInterface
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel<string>> Login(UserLoginDTO userLogin)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                var token = "";
                // 1. Validar usuário e senha
                if (userLogin.UserName != "admin" || userLogin.Password != "12345")
                {
                    response.Data = token;
                    response.Message = "Usuário ou senha incorretos";
                    response.Status = false;

                    return response;
                }

                token = await GenerateJwtToken(userLogin.UserName);
            }
            catch (Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        private async Task<string> GenerateJwtToken(string userName)
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
