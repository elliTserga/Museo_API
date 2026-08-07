namespace MuseoShared.DTOs
{
    public class UpdateExhibitDto
    {
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int Year { get; set; }

        public string ImageUrl { get; set; } = "";

        public int CategoryId { get; set; }

        public bool Visible { get; set; }
    }
}