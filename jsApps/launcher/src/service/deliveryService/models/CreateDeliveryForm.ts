export default interface CreateDeliveryFrom {
    name: string;
    startDate: Date;
    endDate: Date;
    fromLocationHash: string;
    toLocationHash: string;
}

export const GetDefaultCreateDeliveryForm = (): CreateDeliveryFrom =>
    ({
        name: "",
        toLocationHash: "",
        fromLocationHash: "",
    } as CreateDeliveryFrom);
