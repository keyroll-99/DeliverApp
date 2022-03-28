using System.ComponentModel.DataAnnotations;

namespace Models.Request.User;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
