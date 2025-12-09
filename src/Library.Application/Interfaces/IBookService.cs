using Library.Application.DTOs.Book;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookDto?> GetByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> CreateAsync(CreateBookDto bookDto);
        Task<BookDto> UpdateAsync(int id, UpdateBookDto bookDto);
        Task<bool> DeleteAsync(int id);
    }
}