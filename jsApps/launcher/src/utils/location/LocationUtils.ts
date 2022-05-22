import Location from "service/location/models/Location";

export const LocationShortText = (location: Location): string =>
    `${location.postalCode} ${location.city} ${location.street}`;

export const LocationFullText = (location: Location): string =>
    `${location.country} ${location.region} ${location.postalCode} ${location.city} ${location.street} ${location.no}`;
