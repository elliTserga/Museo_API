namespace MuseoShared.Models
{
    public class MediaItem
    {
        public int Id { get; set; }
        public int ExhibitId { get; set; }
        public string FileName { get; set; } = "";
        public string FileType { get; set; } = "";
        public string Url { get; set; } = "";
        public long Size { get; set; }
    }
}
