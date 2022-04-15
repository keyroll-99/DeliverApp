import AuthResponse from "./models/AuthResponse";
import User from "./models/User";

export const MapAuthResponseToUser = (response: AuthResponse): User => {
    return {
        Hash: response.hash,
        JwtToken: response.jwtToken,
        Name: response.name,
        Roles: response.roles,
        Surname: response.surname,
        Username: response.surname,
        ExpireDate: response.expireDate,
    };
};

export default MapAuthResponseToUser;
