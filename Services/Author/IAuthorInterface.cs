using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Author
{
    public interface IAuthorInterface
    {
        Task<ResponseModel<List<AuthorModel>>> ListAuthors();
        Task<ResponseModel<AuthorModel>> GetAuthorById(int idAuthor);
        Task<ResponseModel<AuthorModel>> GetAuthorByBookId(int bookId);
        Task<ResponseModel<List<AuthorModel>>> CreateAuthor(AuthorCreateDTO authorDTO);
        Task<ResponseModel<List<AuthorModel>>> UpdateAuthor(AuthorUpdateDTO authorDTO);
        Task<ResponseModel<List<AuthorModel>>> DeleteAuthor(int idAuthor);
    }
}
