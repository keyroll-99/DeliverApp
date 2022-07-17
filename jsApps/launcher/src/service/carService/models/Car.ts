import BaseUserResponse from "service/userService/models/UserModels/BaseUserResponse";

export default interface Car {
    hash: string;
    registrationNumber: string;
    brand: string;
    model: string;
    vin: string;
    driver?: BaseUserResponse;
}
