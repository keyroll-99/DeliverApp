export default interface ChangeDeliveryStatusForm {
    deliveryHash: string;
    newStatus: number;
}

export const GetDefaultChangeDeliveryStatusForm = (hash: string, currentStatus: number) =>
    ({
        deliveryHash: hash,
        newStatus: currentStatus,
    } as ChangeDeliveryStatusForm);
