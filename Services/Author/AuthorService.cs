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

        public async Task<ResponseModel<AuthorModel>> GetAuthorByBookId(int idBook)
        {
            ResponseModel<AuthorModel> response = new ResponseModel<AuthorModel>();
            try
            {
                var book = await _context.Books
                    .Include(a => a.Author)
                    .FirstOrDefaultAsync(b => b.Id == idBook);

                var author = book != null ? book.Author : null;
                response.Data = author;
                response.Message = author != null ? "Author found!" : "Author not found";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<AuthorModel>> GetAuthorById(int idAuthor)
        {
            ResponseModel<AuthorModel> response = new ResponseModel<AuthorModel>();
            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == idAuthor);

                response.Data = author;
                response.Message = author != null ? "Author found!" : "Author not found";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
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
