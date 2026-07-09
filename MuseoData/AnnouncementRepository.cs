using Adapter;
using Dapper;
using MuseoShared.DTOs;
using MuseoShared.Models;
using System.Data;

namespace MuseoData.Repositories;

public class AnnouncementRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public AnnouncementRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Announcement>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<Announcement>(
            "sp_GetAllAnnouncements",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateAnnouncementDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "sp_CreateAnnouncement",
            dto,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteAsync(
            "sp_DeleteAnnouncement",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }
}