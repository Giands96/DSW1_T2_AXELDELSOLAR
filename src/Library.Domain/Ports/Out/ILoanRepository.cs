using Library.Domain.Entities;

namespace Library.Domain.Ports.Out
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetActiveLoansAsync();
        Task<IEnumerable<Loan>> GetLoansByBookIdAsync(int bookId);
        Task<Loan?> GetActiveLoanByIdAsync(int loanId);
    }
}