﻿using Models.Request.Authentication;
using Models.Request.User;
using Models.Response.Authentication;

namespace Services.Interface;

public interface IAuthenticationService
{
    Task<AuthResponse> Login(LoginRequest loginRequest, string ipAddress);
    Task<AuthResponse> RefreshToken(string? token, string ipAddress);
    Task Logout(string ipAddress);
    Task<PermissionResponse> GetLoggedUserPermission();
}
