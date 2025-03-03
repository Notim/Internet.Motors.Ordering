namespace Core.Domain;

public class Vehicle
{

    public Vehicle(
        Guid vehicleId,
        string? carName, 
        decimal price
    )
    {
        VehicleId = vehicleId;
        CarName = carName;
        Price = price;
    }

    public Guid VehicleId { get; private set; }

    public string? CarName { get; private set; }

    public decimal Price { get; private set; }

}