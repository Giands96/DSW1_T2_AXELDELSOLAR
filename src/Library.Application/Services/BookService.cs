using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Exceptions;
using Library.Domain.Ports.Out;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetWithLoansAsync(id);
            return book == null ? null : _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var existingBook = await _unitOfWork.Books.GetByISBNAsync(dto.ISBN);
            if (existingBook != null)
            {
                throw new DuplicateEntityException("Libro", "ISBN", dto.ISBN);
            }

            var book = _mapper.Map<Book>(dto);
            var createdBook = await _unitOfWork.Books.CreateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<BookDto> UpdateAsync(int id, UpdateBookDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Libro", id);
            }

            var existingBook = await _unitOfWork.Books.GetByISBNAsync(dto.ISBN);
            if (existingBook != null && existingBook.Id != id)
            {
                throw new DuplicateEntityException("Libro", "ISBN", dto.ISBN);
            }
            
            _mapper.Map(dto, book);

            var updatedBook = await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookDto>(updatedBook);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _unitOfWork.Books.GetWithLoansAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Libro", id);
            }

            if (book.Loans.Any(l => l.IsActive()))
            {
                 throw new BusinessRuleException(
                    "ActiveLoansExist",
                    $"No se puede eliminar el libro con ID {id} porque tiene {book.Loans.Count(l => l.Status == "Active")} préstamos activos."
                );
            }

            var result = await _unitOfWork.Books.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }
    }
}