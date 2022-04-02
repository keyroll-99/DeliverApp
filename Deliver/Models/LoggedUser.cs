namespace Models
{
    public class LoggedUser
    {
        public long Id { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
