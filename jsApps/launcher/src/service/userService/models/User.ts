export default interface User {
    hash: string;
    username: string;
    name: string;
    surname: string;
    jwt: string;
    expireDate: Date;
    roles: string[];
}
