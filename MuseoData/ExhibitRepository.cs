using Dapper;
using Adapter;
using System.Data;
using MuseoShared.Models;
using MuseoShared.DTOs;

namespace MuseoData.Repositories;

public class ExhibitRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public ExhibitRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Exhibit>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<Exhibit>(
            "sp_GetAllExhibits",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Exhibit?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Exhibit>(
            "sp_GetExhibitById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateExhibitDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "sp_CreateExhibit",
            dto,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(int id, UpdateExhibitDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteAsync(
            "sp_UpdateExhibit",
            new
            {
                Id = id,
                dto.Title,
                dto.Description,
                dto.Year,
                dto.ImageUrl,
                dto.CategoryId
            },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteAsync(
            "sp_DeleteExhibit",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }
}