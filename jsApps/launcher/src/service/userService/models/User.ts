export default interface User {
    Hash: string;
    Username: string;
    Name: string;
    Surname: string;
    JwtToken: string;
    ExpireDate: Date;
    Roles: string[];
}
