export default interface CreateLocationForm {
    country: string;
    city: string;
    region: string;
    postalCode: string;
    street: string;
    no: string;
    email: string;
    phoneNumber: string;
}

export const GetDefaultCreateLocationForm = (): CreateLocationForm =>
    ({
        city: "",
        country: "",
        email: "",
        no: "",
        phoneNumber: "",
        postalCode: "",
        region: "",
        street: "",
    } as CreateLocationForm);
