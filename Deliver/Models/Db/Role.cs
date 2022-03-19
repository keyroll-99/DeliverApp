namespace Models.Db
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
