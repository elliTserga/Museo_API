namespace MuseoShared.DTOs
{
    public class CreateExhibitDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Year { get; set; }
        public string ImageUrl { get; set; } = "";
        public int CategoryId { get; set; }
    }
}
