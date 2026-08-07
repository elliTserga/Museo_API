using Adapter.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using MuseoShared.Interfaces;

namespace Adapter.Storage
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _settings;

        public MinioStorageService(IOptions<MinioSettings> options)
        {
            _settings = options.Value;

            _minioClient = new MinioClient()
                .WithEndpoint(_settings.Endpoint)
                .WithCredentials(
                    _settings.AccessKey,
                    _settings.SecretKey)
                .WithSSL(_settings.UseSSL)
                .Build();
        }

        public async Task UploadAsync(
            string path,
            Stream stream,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(path)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(
                putObjectArgs,
                cancellationToken);
        }

        public async Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(path);

            await _minioClient.RemoveObjectAsync(
                removeObjectArgs,
                cancellationToken);
        }

        public async Task<string> GetFileUrlAsync(string path)
        {
            var presignedArgs = new PresignedGetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(path)
                .WithExpiry(60 * 60);

            return await _minioClient.PresignedGetObjectAsync(
                presignedArgs);
        }
    }
}