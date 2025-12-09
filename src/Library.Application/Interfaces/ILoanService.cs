using Library.Application.DTOs.Loan;

namespace Library.Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanDto?> GetByIdAsync(int id);
        Task<IEnumerable<LoanDto>> GetActiveLoansAsync();
        Task<LoanDto> CreateAsync(CreateLoanDto loanDto);
        Task<LoanDto> ReturnLoanAsync(int id);
    }
}