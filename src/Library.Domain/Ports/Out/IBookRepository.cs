using Library.Domain.Entities;

namespace Library.Domain.Ports.Out
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
        Task<IEnumerable<Book>> SearchByTitleOrAuthorAsync(string searchTerm);
        Task<Book?> GetWithLoansAsync(int id);
    }
}