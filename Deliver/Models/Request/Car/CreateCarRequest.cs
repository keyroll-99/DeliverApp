namespace Models.Request.Car;

public class CreateCarRequest
{
    public string RegistrationNumber { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Vin { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(RegistrationNumber)
        && !string.IsNullOrWhiteSpace(Brand)
        && !string.IsNullOrWhiteSpace(Model)
        && !string.IsNullOrWhiteSpace(Vin);
}


