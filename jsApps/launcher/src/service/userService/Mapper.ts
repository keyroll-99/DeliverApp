import AuthResponse from "./models/AuthResponse";
import User from "./models/User";

export const MapAuthResponseToUser = (response: AuthResponse): User => {
    return {
        hash: response.hash,
        jwt: response.jwt,
        name: response.name,
        roles: response.roles,
        surname: response.surname,
        username: response.surname,
        expireDate: response.expireDate,
    };
};

export default MapAuthResponseToUser;
