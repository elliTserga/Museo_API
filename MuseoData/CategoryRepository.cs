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
}