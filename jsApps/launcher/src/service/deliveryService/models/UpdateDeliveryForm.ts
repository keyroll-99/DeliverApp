import Delivery from "./Delivery";

export default interface UpdateDeliveryFrom {
    deliveryHash: string;
    name: string;
    startDate: Date;
    endDate: Date;
    fromLocationHash: string;
    toLocationHash: string;
}

export const GetDefaultUpdateDeliveryForm = (delivery: Delivery): UpdateDeliveryFrom =>
    ({
        deliveryHash: delivery.hash,
        endDate: delivery.endDate,
        fromLocationHash: delivery.from.hash,
        name: delivery.name,
        startDate: delivery.startDate,
        toLocationHash: delivery.to.hash,
    } as UpdateDeliveryFrom);
