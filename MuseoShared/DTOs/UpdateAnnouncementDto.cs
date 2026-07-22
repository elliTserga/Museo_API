namespace MuseoShared.DTOs;

public class UpdateAnnouncementDto
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool Visible { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}