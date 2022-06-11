namespace Models.Db;

public class User : BaseHashModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsFired { get; set; }

    public long? CompanyId { get; set; }
    public Company Company { get; set; }

    public Car Car { get; set; }

    public virtual ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
