using Adapter;
using Dapper;
using MuseoShared.DTOs;
using MuseoShared.Models;
using System.Data;

namespace MuseoData.Repositories;

public class MediaRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public MediaRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<MediaItem>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<MediaItem>(
            "sp_GetAllMediaItems",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<MediaItem?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<MediaItem>(
            "sp_GetMediaItemById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<MediaItem>> GetByExhibitIdAsync(int exhibitId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<MediaItem>(
            "sp_GetMediaByExhibitId",
            new { ExhibitId = exhibitId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateMediaItemDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "sp_CreateMediaItem",
            dto,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(int id, UpdateMediaItemDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteScalarAsync<int>(
            "sp_UpdateMediaItem",
            new
            {
                Id = id,
                dto.ExhibitId,
                dto.FileName,
                dto.FileType,
                dto.Url
            },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteScalarAsync<int>(
            "sp_DeleteMediaItem",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }
}