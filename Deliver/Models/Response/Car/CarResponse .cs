using Models.Response.User;

namespace Models.Response.Car;

public class CarResponse
{
    public Guid Hash { get; set; }
    public string RegistrationNumber { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Vin { get; set; }
    public BaseUserResponse Driver { get; set; }
}
