using Microsoft.EntityFrameworkCore;
using WebAPI_DOTNET8.Data;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Author
{
    public class AuthorService : IAuthorInterface
    {
        private readonly AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseModel<AuthorModel>> GetAuthorByBookId(int bookId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<AuthorModel>> GetAuthorById(int idAuthor)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<List<AuthorModel>>> ListAuthors()
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try 
            {
                var authors = await _context.Authors.ToListAsync();

                response.Data = authors;
                response.Message = "All authors were collected!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }
    }
}
