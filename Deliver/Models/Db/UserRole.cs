namespace Models.Db
{
    public class UserRole : BaseModel
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}
