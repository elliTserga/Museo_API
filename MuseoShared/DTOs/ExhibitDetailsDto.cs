using MuseoShared.Models;

namespace MuseoShared.DTOs;

public class ExhibitDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Year { get; set; }

    public int? CategoryId { get; set; }

    public bool Visible { get; set; }

    public string? ImagePath { get; set; }

    public string? ImageUrl { get; set; }

    public List<MediaItem> Media { get; set; } = new();
}