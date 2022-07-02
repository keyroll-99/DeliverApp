using Models.Response.User;

namespace Models.Response.Car;

public class Car
{
    public Guid Hash { get; set; }
    public string RegistrationNumber { get; set; }
    public UserResponse Driver { get; set; }
}
