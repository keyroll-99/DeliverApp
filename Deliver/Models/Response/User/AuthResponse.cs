using System.Text.Json.Serialization;

namespace Models.Response.User;

public class AuthResponse
{
    public Guid Hash { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Jwt { get; set; }
    public DateTime ExpireDate { get; set; }
    public List<string> Roles { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }
}
