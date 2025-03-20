using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Book
{
    public interface IBookInterface
    {
        Task<ResponseModel<List<BookModel>>> ListBooks();
        Task<ResponseModel<BookModel>> GetBookById(int idBook);
        Task<ResponseModel<List<BookModel>>> GetBooksByAuthorId(int idAuthor);
        Task<ResponseModel<List<BookModel>>> CreateBook(BookCreateDTO bookDTO);
        Task<ResponseModel<List<BookModel>>> UpdateBook(BookUpdateDTO bookDTO);
        Task<ResponseModel<List<BookModel>>> DeleteBook(int idBook);
    }
}
