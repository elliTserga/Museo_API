namespace MuseoShared.Interfaces
{
    public interface IStorageService
    {
        Task UploadAsync(
            string path,
            Stream stream,
            string contentType,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default
        );

        Task<string> GetFileUrlAsync(string path);
    }
}