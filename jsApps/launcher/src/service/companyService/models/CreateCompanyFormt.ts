export default interface CreateCompanyForm {
    name: string;
    email: string;
    phoneNumber: string;
}

export const GetDefaultCreateCompanyForm = (): CreateCompanyForm => ({
    email: "",
    name: "",
    phoneNumber: "",
});
