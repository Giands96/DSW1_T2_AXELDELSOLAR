using Microsoft.EntityFrameworkCore;
using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Library.Infraestructure.Persistence.Context;

namespace Library.Infraestructure.Persistence.Repositories
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            return await _dbSet
                .Where(l => l.Status == "Active")
                .Include(l => l.Book)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Where(l => l.BookId == bookId)
                .Include(l => l.Book)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
        }

        public async Task<Loan?> GetActiveLoanByIdAsync(int loanId)
        {
            return await _dbSet
                .Where(l => l.Id == loanId && l.Status == "Active")
                .Include(l => l.Book)
                .FirstOrDefaultAsync();
        }
    }
}