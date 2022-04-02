namespace Models.Db;

public class RefreshToken : BaseModel
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool IsUsed { get; set; } = false;
    public string UsedBy { get; set; }
    public string CreatedBy { get; set; }
    public string ReplacedByToken { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}