export default interface AuthResponse {
    hash: string;
    username: string;
    name: string;
    surname: string;
    jwt: string;
    expireDate: Date;
    roles: string[];
}
