namespace Models.Db;

public class User : BaseModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public long CompanyId { get; set; }
    public Company Company { get; set; }
}
