using Microsoft.EntityFrameworkCore;
using WebAPI_DOTNET8.Data;
using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Book
{
    public class BookService : IBookInterface
    {
        private readonly AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<BookModel>>> CreateBook(BookCreateDTO bookDTO)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                var book = new BookModel()
                {
                    Title = bookDTO.Title,
                    Author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == bookDTO.IdAuthor)
                };

                _context.Add(book);
                await _context.SaveChangesAsync();

                response.Data = await _context.Books.Include(b => b.Author).ToListAsync();
                response.Message = "Book created successfully!";
            }
            catch (Exception ex) 
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<List<BookModel>>> DeleteBook(int idBook)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(a => a.Id == idBook);
                if (book == null)
                {
                    response.Message = "No books found";
                    return response;
                }

                _context.Remove(book);
                await _context.SaveChangesAsync();

                response.Data = await _context.Books.Include(b => b.Author).ToListAsync();
                response.Message = "Book successfully removed!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<BookModel>> GetBookById(int idBook)
        {
            ResponseModel<BookModel> response = new ResponseModel<BookModel>();
            try
            {
                var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == idBook);

                response.Data = book;
                response.Message = book != null ? "Book found!" : "Book not found";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false; 
            }

            return response;
        }

        public async Task<ResponseModel<List<BookModel>>> ListBooks()
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                response.Data = await _context.Books.Include(b => b.Author).ToListAsync();
                response.Message = "All books were collected!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<List<BookModel>>> UpdateBook(BookUpdateDTO bookDTO)
        {
            ResponseModel<List<BookModel>> response = new ResponseModel<List<BookModel>>();
            try
            {
                var book = _context.Books.FirstOrDefault(a => a.Id == bookDTO.Id);
                if (book == null)
                {
                    response.Message = "No books found";
                    return response;
                }

                book.Title = bookDTO.Title;
                book.Author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == bookDTO.IdAuthor);

                _context.Update(book);
                await _context.SaveChangesAsync();

                response.Data = await _context.Books.Include(b => b.Author).ToListAsync();
                response.Message = "Book edited successfully!";
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
