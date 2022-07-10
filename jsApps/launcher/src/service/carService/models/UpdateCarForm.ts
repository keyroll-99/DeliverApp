import Car from "./Car";

export default interface UpdateCarForm {
    hash: string;
    registrationNumber: string;
    brand: string;
    model: string;
    vin: string;
}

export const GetUpdateCarForm = (car: Car): UpdateCarForm => ({
    brand: car.brand,
    hash: car.hash,
    model: car.model,
    registrationNumber: car.registrationNumber,
    vin: car.vin,
});
