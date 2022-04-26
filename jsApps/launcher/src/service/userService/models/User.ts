export default interface User {
    hash: string;
    username: string;
    name: string;
    surname: string;
    jwt: string;
    companyHash: string;
    expireDate: Date;
    roles: string[];
}
