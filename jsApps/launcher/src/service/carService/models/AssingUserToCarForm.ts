export default interface AssingUserToCarForm {
    carHash: string;
    userHash: string;
}

export const GetDefaultAssingUserToCarForm = (carHash: string, driverHash?: string): AssingUserToCarForm => ({
    carHash: carHash,
    userHash: driverHash ?? "",
});
