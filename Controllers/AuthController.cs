using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;
using WebAPI_DOTNET8.Services.Auth;

namespace WebAPI_DOTNET8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;

        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            // Validar model recebida
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            // Validar login do usuário
            var token = await _authInterface.Login(userLogin);
            if (!token.Status) 
                return Unauthorized(token.Message);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserCreateDTO>> CreateUser(UserCreateDTO userCreateDTO)
        {
            // Validar model
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authInterface.CreateUser(userCreateDTO);
            if(!response.Status)
                return BadRequest(response.Message);

            return Ok(response);
        }
    }
}
