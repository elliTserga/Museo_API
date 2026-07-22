using Adapter;
using Dapper;
using MuseoShared.DTOs;
using MuseoShared.Models;
using System.Data;

namespace MuseoData.Repositories;

public class CategoryRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public CategoryRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<Category>(
            "sp_GetAllCategories",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Category>(
            "sp_GetCategoryById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(CreateCategoryDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "sp_CreateCategory",
            dto,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteScalarAsync<int>(
            "sp_UpdateCategory",
            new
            {
                Id = id,
                dto.Name
            },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        int rowsAffected = await connection.ExecuteScalarAsync<int>(
            "sp_DeleteCategory",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }
}