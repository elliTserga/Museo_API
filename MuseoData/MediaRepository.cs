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
}