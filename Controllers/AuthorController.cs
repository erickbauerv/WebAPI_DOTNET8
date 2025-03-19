using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_DOTNET8.DTOs;
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

        [HttpGet("GetAuthorById/{idAuthor}")]
        public async Task<ActionResult<ResponseModel<AuthorModel>>> GetAuthorById(int idAuthor)
        {
            var author = await _authorInterface.GetAuthorById(idAuthor);
            return Ok(author);
        }

        [HttpGet("GetAuthorByBookId/{idBook}")]
        public async Task<ActionResult<ResponseModel<AuthorModel>>> GetAuthorByBookId(int idBook)
        {
            var author = await _authorInterface.GetAuthorByBookId(idBook);
            return Ok(author);
        }

        [HttpPost("CreateAuthor")]
        public async Task<ActionResult<ResponseModel<AuthorModel>>> CreateAuthor(AuthorDTO authorDTO)
        {
            var authors = await _authorInterface.CreateAuthor(authorDTO);
            return Ok(authors);
        }
    }
}
