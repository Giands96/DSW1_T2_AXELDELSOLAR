using AutoMapper;
using Library.Application.DTOs.Loan;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;


namespace Library.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoanDto?> GetByIdAsync(int id)
        {
            var loan = await _unitOfWork.Loans.GetByIdAsync(id);
            return loan == null ? null : _mapper.Map<LoanDto>(loan);
        }

        public async Task<IEnumerable<LoanDto>> GetActiveLoansAsync()
        {
            var loans = await _unitOfWork.Loans.GetActiveLoansAsync();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var book = await _unitOfWork.Books.GetByIdAsync(dto.BookId);
                if (book == null)
                {
                    throw new NotFoundException("Libro", dto.BookId);
                }

                book.DecreaseStock(); 

                await _unitOfWork.Books.UpdateAsync(book);

                var loan = _mapper.Map<Loan>(dto);
                var createdLoan = await _unitOfWork.Loans.CreateAsync(loan);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync(); // Confirma ambas operaciones

                var loanWithBook = await _unitOfWork.Loans.GetByIdAsync(createdLoan.Id);
                return _mapper.Map<LoanDto>(loanWithBook!);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<LoanDto> ReturnLoanAsync(int loanId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var loan = await _unitOfWork.Loans.GetActiveLoanByIdAsync(loanId);
                if (loan == null)
                {
                    throw new NotFoundException("Préstamo Activo", loanId);
                }

                if (loan.Book == null)
                {
                    throw new DomainException($"Libro asociado al préstamo {loanId} no encontrado.");
                }

                loan.Status = "Returned";
                loan.ReturnDate = DateTime.Now;
                await _unitOfWork.Loans.UpdateAsync(loan);

                loan.Book.IncreaseStock();
                await _unitOfWork.Books.UpdateAsync(loan.Book);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<LoanDto>(loan);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(); 
                throw;
            }
        }
    }
}