using AutoMapper;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Loan;
using Library.Domain.Entities;

namespace Library.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos de Libro
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.ActiveLoansCount, opt => opt.MapFrom(src => src.Loans != null ? src.Loans.Count(l => l.Status == "Active") : 0));
                
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
                
            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Mapeos de Préstamo
            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty));
                
            CreateMap<CreateLoanDto, Loan>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}