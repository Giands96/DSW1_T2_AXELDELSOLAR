using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Domain.Entities;

namespace Library.Infraestructure.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.StudentName).IsRequired().HasMaxLength(150);
            builder.Property(l => l.LoanDate).IsRequired();
            
            // ReturnDate es nullable
            builder.Property(l => l.ReturnDate).IsRequired(false); 

            // Status (Active, Returned)
            builder.Property(l => l.Status).IsRequired().HasMaxLength(20);
            
            builder.Property(l => l.CreatedAt).IsRequired();
            
            builder.HasIndex(l => l.Status);
            builder.HasIndex(l => l.BookId);
        }
    }
}