namespace Models.Request.Car;

public class AssignUserToCarRequest
{
    public Guid CarHash { get; set; }
    public Guid UserHash { get; set; }
}


