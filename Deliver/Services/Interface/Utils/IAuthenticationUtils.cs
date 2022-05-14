using Models.Db;

namespace Services.Interface.Utils;

public interface IAuthenticationUtils
{
    string GenerateJwtToken(User user);
    int? ValidateJwtToken(string? token);
    Task<RefreshToken> GenerateRefreshToken(User user, string ipAddress);
    Task<RefreshToken?> GetRefreshTokenByToken(string? token);
    Task RevokeAllRefreshTokenForUser(User user, string ipAddress);
    Task RevokeRefreshToken(RefreshToken refreshToken, string ipAddress);
}
