using Library.Domain.Exceptions;

namespace Library.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Stock {get; set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        public void DecreaseStock()
        {
            if (Stock <= 0)
            {
                throw new BusinessRuleException("StockZero", $"No se puede prestar el libro '{Title}' porque no hay stock disponible.");
            }
            Stock--;
        }

        public void IncreaseStock()
        {
            Stock++;
        }
    }
}