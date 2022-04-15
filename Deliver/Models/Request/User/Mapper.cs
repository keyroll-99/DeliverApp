namespace Models.Request.User
{
    public static class Mapper
    {
        public static Db.User AsUser(this CreateUserRequest createUserRequest)
            => new()
            {
                Name = createUserRequest.Name,
                Surname = createUserRequest.Surname,
                Email = createUserRequest.Email,
                PhoneNumber = createUserRequest.PhoneNumber,
                Username = createUserRequest.Username,
            };
    }
}
