using Microsoft.AspNetCore.Mvc;

namespace MuseoAPI.Requests;

public class CreateMediaItemRequest
{
    [FromForm(Name = "exhibitId")]
    public int ExhibitId { get; set; }

    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;
}