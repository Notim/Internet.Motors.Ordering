using System.Data;
using Dapper;
using Infrastructure.Data.Configs;
using Infrastructure.Data.Dao.Dto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.Dao;

public class VehicleOrderDao : IVehicleOrderDao
{

    private readonly DatabaseConfig _config;

    public VehicleOrderDao(IOptions<DatabaseConfig> configOptions)
    {
        _config = configOptions.Value;
    }

    public async Task<VehicleOrderDto?> GetOrderByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        
        const string query = @"
            SELECT * 
            FROM VEHICLE_ORDER 
            WHERE VehicleId = @VehicleId
        ";

        return await connection.QueryFirstOrDefaultAsync<VehicleOrderDto>(
            new CommandDefinition(
                query,
                new
                {
                    VehicleId = vehicleId
                },
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<int> InsertVehicleOrderAsync(VehicleOrderDto dto, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        
        const string query = @"
            INSERT INTO VEHICLE_ORDER (
                OrderId,
                CustomerDocument, 
                OrderedAt,
                VehicleId,
                CarName,
                Price,
                Status,
                Observation
            )
            VALUES (
                @OrderId,
                @CustomerDocument,
                @OrderedAt,
                @VehicleId,
                @CarName,
                @Price,
                @Status,
                @Observation
            )
        ";

        return await connection.ExecuteAsync(new CommandDefinition(query, dto, cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateVehicleOrderAsync(VehicleOrderDto dto, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
            
        const string query = @"
            UPDATE VEHICLE_ORDER
            SET 
                Status = @Status,
                Observation = @Observation
            WHERE 
                Id = @Id
        ";

        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(query, dto, cancellationToken: cancellationToken));

        return affectedRows > 0;
    }
    
    private IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_config.ConnectionString);
        connection.Open();

        return connection;
    }

}