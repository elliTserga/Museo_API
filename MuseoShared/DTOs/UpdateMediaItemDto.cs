namespace MuseoShared.DTOs;

public class UpdateMediaItemDto
{
    public int ExhibitId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;
}