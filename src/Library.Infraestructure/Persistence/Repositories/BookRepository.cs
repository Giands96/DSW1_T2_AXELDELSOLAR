using Microsoft.EntityFrameworkCore;
using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Library.Infraestructure.Persistence.Context;

namespace Library.Infraestructure.Persistence.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        // Metodo asincrono para obtener un libro por su ISBN
        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> SearchByTitleOrAuthorAsync(string searchTerm)
        {
            return await _dbSet
                .Where(b => EF.Functions.Like(b.Title, $"%{searchTerm}%") ||
                            EF.Functions.Like(b.Author, $"%{searchTerm}%"))
                .ToListAsync();
        }

        public async Task<Book?> GetWithLoansAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Loans)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}