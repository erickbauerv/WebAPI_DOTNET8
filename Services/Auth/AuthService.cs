using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                // Buscar usuário no banco de dados
                var user = await _context.UserLogin.FirstOrDefaultAsync(u => u.UserName == userLogin.UserName);

                // Verificar se usuário existe e Verificar senha (com hash seguro)
                if (user == null || !VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("Usuário ou senha incorretos");

                // Gerar token JWT
                var token = GenerateJwtToken(user);

                response.Data = token;
                response.Message = "Usuário logado com sucesso";
            }
            catch (Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i])
                    return false;
            }

            return true;
        }

        private string GenerateJwtToken(UserLoginModel user)
        {
            // Configurar as informações do token Jwt (Claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwtConfig = _configuration.GetSection("Jwt");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? jwtConfig["Key"] ?? throw new InvalidOperationException("Nenhuma chave JWT configurada");

            // Criar chave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configurar token
            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig["ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            // Gerar e retornar o token com string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponseModel<UserCreateDTO>> CreateUser(UserCreateDTO userCreateDTO)
        {
            ResponseModel<UserCreateDTO> response = new ResponseModel<UserCreateDTO>();
            try
            {
                // Verificar se usarname já existe
                if (await _context.UserLogin.AnyAsync(u => u.UserName == userCreateDTO.UserName))
                {
                    throw new Exception("Nome de usuário já existe");
                }

                // Criar hash da senha
                CreatePasswordHash(userCreateDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                // Criar novo usuário
                var user = new UserLoginModel
                {
                    UserName = userCreateDTO.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                // Salvar no banco
                _context.UserLogin.Add(user);
                await _context.SaveChangesAsync();

                response.Message = "Usuário criado com sucesso";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt =  hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
