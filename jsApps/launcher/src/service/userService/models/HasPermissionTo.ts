import { PermisionToActionEnum, PermissionTo } from "./Permissions";

export default interface HasPermissionTo {
    permissionTo: PermissionTo;
    permissionAction: PermisionToActionEnum;
}
