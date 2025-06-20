namespace MiMangaBot.Domain.Models
{
    public class PrestamoDto
    {
        public string? Name_Customer { get; set; }
        public int? MangaId { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

    public class PrestamoGetDto
    {
        public int ID { get; set; }
        public string? Name_Customer { get; set; }
        public int? MangaId { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string MangaName { get; set; } = string.Empty;
    }
} 