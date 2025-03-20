using Microsoft.AspNetCore.Mvc;
using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;
using WebAPI_DOTNET8.Services.Book;

namespace WebAPI_DOTNET8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookInterface _bookInterface;

        public BookController(IBookInterface bookInterface)
        {
            _bookInterface = bookInterface;
        }

        [HttpGet("ListBooks")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> ListBooks()
        {
            var books = await _bookInterface.ListBooks();
            return Ok(books);
        }

        [HttpGet("GetBookById/{idBook}")]
        public async Task<ActionResult<ResponseModel<BookModel>>> GetBookById(int idBook)
        {
            var book = await _bookInterface.GetBookById(idBook);
            return Ok(book);
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> CreateBook(BookCreateDTO bookDTO)
        {
            var books = await _bookInterface.CreateBook(bookDTO);
            return Ok(books);
        }

        [HttpPut("UpdateBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> UpdateBook(BookUpdateDTO bookDTO)
        {
            var books = await _bookInterface.UpdateBook(bookDTO);
            return Ok(books);
        }

        [HttpDelete("DeleteBook")]
        public async Task<ActionResult<ResponseModel<List<BookModel>>>> DeleteBook(int idBook)
        {
            var books = await _bookInterface.DeleteBook(idBook);
            return Ok(books);
        }
    }
}
