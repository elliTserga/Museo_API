namespace MuseoShared.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;

    public string? ImagePath { get; set; }
}