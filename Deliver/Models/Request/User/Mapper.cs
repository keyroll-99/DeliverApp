using Models.Response.Authentication;

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

        public static Db.User UpdateUser(this UpdateUserRequest updateUser, Db.User user)
        {
            user.Email = updateUser.Email;
            user.PhoneNumber = updateUser.PhoneNumber;
            user.Name = updateUser.Name;
            user.Surname = updateUser.Surname;
            return user;
        }
    }
}
