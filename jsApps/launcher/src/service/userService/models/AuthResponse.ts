export default interface AuthResponse {
    hash: string;
    username: string;
    name: string;
    surname: string;
    jwtToken: string;
    expireDate: Date;
    roles: string[];
}
