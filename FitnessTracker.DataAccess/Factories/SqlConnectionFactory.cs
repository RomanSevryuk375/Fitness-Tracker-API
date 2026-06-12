using FitnessTracker.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FitnessTracker.DataAccess.Factories;

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("FitnessDbContext");
        return new NpgsqlConnection(connectionString);
    }
}
