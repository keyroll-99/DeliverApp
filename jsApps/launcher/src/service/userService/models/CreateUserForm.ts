export default interface CreateUserForm {
    name: string;
    surname: string;
    email: string;
    phoneNumber?: string;
    username: string;
    companyHash: string;
    roleIds: number[];
}
