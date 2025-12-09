using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.Domain.Entities;

namespace Library.Infraestructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Author).IsRequired().HasMaxLength(150);
            
            // ISBN debe ser unico
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            builder.HasIndex(b => b.ISBN).IsUnique(); 

            builder.Property(b => b.Stock).IsRequired();
            builder.Property(b => b.CreatedAt).IsRequired();

            // relación uno a muchos Book tiene muchos prestamos
            builder.HasMany(b => b.Loans)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}