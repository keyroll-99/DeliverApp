import { UseStore } from "stores/Store";
import HasPermissionTo from "./models/HasPermissionTo";
import { PermisionToActionEnum } from "./models/Permissions";

export const HasPermission = (permisionTo: HasPermissionTo): boolean => {
    const { userStore } = UseStore();
    if (permisionTo.permissionTo === "car") {
        console.log(permisionTo);
    }

    const permission: PermisionToActionEnum[] = userStore.getPermissions
        ? userStore.getPermissions[permisionTo.permissionTo] ?? []
        : [];

    return permission.findIndex((x) => x === permisionTo.permissionAction) !== -1;
};
