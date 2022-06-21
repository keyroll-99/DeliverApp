export type PermissionTo = "user" | "location" | "company" | "delivery";

export enum PermisionToActionEnum {
    create = 1,
    update = 2,
    get = 3,
    delete = 4,
    assign = 5,
}

export interface Permission {
    user: PermisionToActionEnum[];
    location: PermisionToActionEnum[];
    company: PermisionToActionEnum[];
    delivery: PermisionToActionEnum[];
}
