import {
    Add,
    AddLocationAlt,
    AdminPanelSettings,
    LocalShipping,
    LocationOn,
    Person,
    PersonAdd,
    SupervisorAccount,
    DirectionsCar,
} from "@mui/icons-material";
import { List } from "@mui/material";
import { PermisionToActionEnum } from "service/userService/models/Permissions";
import Path from "utils/route/Path";
import LogoutButton from "./navbarElement/LogoutButton";
import NavButton from "./navbarElement/NavButton";

const MenuList = () => {
    return (
        <List className="navbar-list">
            <NavButton
                text="Delivery list"
                requirePermission={{ permissionAction: PermisionToActionEnum.get, permissionTo: "deliver" }}
                targetLocation={Path.deliveryList}
                icon={<LocalShipping />}
            />
            <NavButton
                text="Add delivery"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "deliver" }}
                targetLocation={Path.deliveryCreate}
                icon={<Add />}
            />
            <NavButton
                text="Locations"
                requirePermission={{ permissionAction: PermisionToActionEnum.get, permissionTo: "location" }}
                targetLocation={Path.locationList}
                icon={<LocationOn />}
            />
            <NavButton
                text="Add location"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "location" }}
                targetLocation={Path.locationAdd}
                icon={<AddLocationAlt />}
            />
            <NavButton
                text="Workers"
                requirePermission={{ permissionAction: PermisionToActionEnum.get, permissionTo: "user" }}
                targetLocation={Path.workersList}
                icon={<SupervisorAccount />}
            />
            <NavButton
                text="Add worker"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "user" }}
                targetLocation={Path.addWorker}
                icon={<PersonAdd />}
            />
            <NavButton text="Account" targetLocation={Path.account} icon={<Person />} />
            <NavButton
                text="Add car"
                targetLocation={Path.car.create}
                icon={<Add />}
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "car" }}
            />
            <NavButton
                text="Cars"
                targetLocation={Path.car.list}
                icon={<DirectionsCar />}
                requirePermission={{ permissionAction: PermisionToActionEnum.get, permissionTo: "car" }}
            />
            <NavButton
                text="Admin Panel"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "company" }}
                targetLocation={Path.admin.createCompany}
                icon={<AdminPanelSettings />}
            />
            <LogoutButton />
        </List>
    );
};

export default MenuList;
