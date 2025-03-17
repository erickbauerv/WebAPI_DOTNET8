using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_DOTNET8.Models;
using WebAPI_DOTNET8.Services.Author;

namespace WebAPI_DOTNET8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorInterface _authorInterface;

        public AuthorController(IAuthorInterface authorInterface)
        {
            _authorInterface = authorInterface;
        }

        [HttpGet("ListAuthors")]
        public async Task<ActionResult<ResponseModel<List<AuthorModel>>>> ListAuthors()
        {
            var authors = await _authorInterface.ListAuthors();
            return Ok(authors);
        }

    }
}
