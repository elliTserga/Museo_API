namespace MuseoShared.Models
{
    public class Exhibit
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int Year { get; set; }

        public int? CategoryId { get; set; }

        public string? ImagePath { get; set; }

        public bool Visible { get; set; }
    }
}