using Adapter;
using Dapper;
using MuseoShared.DTOs;
using MuseoShared.Models;
using System.Data;

namespace MuseoData.Repositories;

public class MuseumRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public MuseumRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Museum?> GetAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Museum>(
            "sp_GetMuseum",
            commandType: CommandType.StoredProcedure);
    }
}