export enum DeliveryStatus {
    new = 1,
    inProgress = 2,
    finished = 3,
}

export const stringDeliveryStatus = (status: number): string => {
    switch (status) {
        case DeliveryStatus.new:
            return "new";
        case DeliveryStatus.inProgress:
            return "in progress";
        case DeliveryStatus.finished:
            return "finished";
        default:
            return "unknow";
    }
};
