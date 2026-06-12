using System.Data;

namespace FitnessTracker.Core.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
