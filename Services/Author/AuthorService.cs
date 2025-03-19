using Microsoft.EntityFrameworkCore;
using WebAPI_DOTNET8.Data;
using WebAPI_DOTNET8.DTOs;
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

        public async Task<ResponseModel<List<AuthorModel>>> CreateAuthor(AuthorCreateDTO authorDTO)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try
            {
                var author = new AuthorModel()
                {
                    Name = authorDTO.Name,
                    LastName = authorDTO.LastName,
                };

                _context.Add(author);
                await _context.SaveChangesAsync();

                response.Data = await _context.Authors.ToListAsync();
                response.Message = "Author created successfully!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<List<AuthorModel>>> DeleteAuthor(int idAuthor)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try
            {
                var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == idAuthor);
                if (author == null)
                {
                    response.Message = "No authors found";
                    return response;
                }

                _context.Remove(author);
                await _context.SaveChangesAsync();

                response.Data = await _context.Authors.ToListAsync();
                response.Message = "Author successfully removed!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
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
                response.Data = await _context.Authors.ToListAsync();
                response.Message = "All authors were collected!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<List<AuthorModel>>> UpdateAuthor(AuthorUpdateDTO authorDTO)
        {
            ResponseModel<List<AuthorModel>> response = new ResponseModel<List<AuthorModel>>();
            try
            {
                var author = _context.Authors.FirstOrDefault(a => a.Id == authorDTO.Id);
                if (author == null)
                {
                    response.Message = "No authors found";
                    return response;
                }

                author.Name = authorDTO.Name;
                author.LastName = authorDTO.LastName;

                _context.Update(author);
                await _context.SaveChangesAsync();

                response.Data = await _context.Authors.ToListAsync();
                response.Message = "Author edited successfully!";
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
