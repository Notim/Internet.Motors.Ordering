namespace Infrastructure.Data.Dao.Dto;

public record VehicleOrderDto
{

    public long Id { get; set; }

    public Guid OrderId { get; set; }

    public string CustomerDocument { get; set; } = string.Empty;

    public DateTime OrderedAt { get; set; }

    public Guid VehicleId { get; set; }

    public string? CarName { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Observation { get; set; }

}