using Adapter;
using Dapper;
using MuseoShared.DTOs;
using MuseoShared.Models;
using System.Data;

namespace MuseoData.Repositories;

public class AuthRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public AuthRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<User?> GetUserAsync(string username)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "sp_GetUserByUsername",
            new { Username = username },
            commandType: CommandType.StoredProcedure);
    }
}