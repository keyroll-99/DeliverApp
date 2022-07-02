namespace Models.Db;

public class PasswordRecovery : BaseHashModel
{
    public long UserId { get; set; }
    public User User { get; set; }
}
