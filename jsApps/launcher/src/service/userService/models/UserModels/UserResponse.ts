export default interface UserResponse {
    hash: string;
    username: string;
    name: string;
    surname: string;
    roles: string[];
    companyName: string;
    companyHash: string;
    phoneNumber?: string;
    email: string;
}
