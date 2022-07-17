export default interface AssignCarToDeliveryForm {
    carHash: string;
    deliveryHash: string;
}

export const GetDefaultAssignCarToDeliveryForm = (deliveryHash: string, carHash?: string): AssignCarToDeliveryForm => ({
    carHash: carHash ?? "",
    deliveryHash: deliveryHash,
});
