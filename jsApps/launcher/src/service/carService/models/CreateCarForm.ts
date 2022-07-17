export default interface CreateCarForm {
    registrationNumber: string;
    brand: string;
    model: string;
    vin: string;
}

export const GetDefaultCreateCarForm = (): CreateCarForm => ({ brand: "", model: "", registrationNumber: "", vin: "" });
