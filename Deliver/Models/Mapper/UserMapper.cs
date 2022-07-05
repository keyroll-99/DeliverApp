using Models.Request.User;
using Models.Response.User;

namespace Models.Mapper;

public static class UserMapper
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

    public static BaseUserResponse AsBaseUserResponse(this Db.User user)
    {
        return new BaseUserResponse
        {
            Email = user.Email,
            Hash = user.Hash,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Surname = user.Surname,
            Username = user.Username,
        };
    }

    public static UserResponse AsUserReponse(this Db.User user)
    {
        if(user.Company is null)
        {
            throw new ArgumentNullException($"missing {nameof(user.Company)}");
        }

        if(user.UserRole is null || !user.UserRole.Any())
        {
            throw new ArgumentNullException($"missing {nameof(user.UserRole)}");
        }

        return new()
        {
            CompanyHash = user.Company.Hash,
            CompanyName = user.Company.Name,
            Email = user.Email,
            Hash = user.Hash,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            Roles = user.UserRole.Select(x => x.Role.Name).ToList(),
            Surname = user.Surname,
            Username = user.Username
        };
    }
}
