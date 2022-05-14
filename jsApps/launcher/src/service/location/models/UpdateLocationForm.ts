import CreateLocationForm from "./CreateLocationForm";

interface UpdateLocationForm extends CreateLocationForm {
    hash: string;
}

export default UpdateLocationForm;

export const GetDefaultCreateLocationForm = (hash: string): UpdateLocationForm =>
    ({
        city: "",
        country: "",
        email: "",
        no: "",
        phoneNumber: "",
        postalCode: "",
        region: "",
        street: "",
        hash: hash,
    } as UpdateLocationForm);
