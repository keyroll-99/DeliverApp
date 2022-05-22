import Location from "service/location/models/Location";

export default interface Delivery {
    hash: string;
    name: string;
    status: number;
    startDate: Date;
    endDate: Date;
    from: Location;
    to: Location;
}
