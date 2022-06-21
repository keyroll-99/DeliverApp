import {
    Add,
    AddLocationAlt,
    AdminPanelSettings,
    LocalShipping,
    LocationOn,
    Person,
    PersonAdd,
    SupervisorAccount,
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
                requirePermission={{ permissionAction: PermisionToActionEnum.get, permissionTo: "delivery" }}
                targetLocation={Path.deliveryList}
                icon={<LocalShipping />}
            />
            <NavButton
                text="Add delivery"
                // roles={roles}
                targetLocation={Path.deliveryCreate}
                icon={<Add />}
                // requireRole={RequrieRoles.Delivery.Create}
            />
            <NavButton
                text="Locations"
                // roles={roles}
                targetLocation={Path.locationList}
                icon={<LocationOn />}
                // requireRole={RequrieRoles.Location.List}
            />
            <NavButton
                text="Add location"
                // roles={roles}
                targetLocation={Path.locationAdd}
                icon={<AddLocationAlt />}
                // requireRole={RequrieRoles.Location.Create}
            />
            <NavButton
                text="Workers"
                // roles={roles}
                targetLocation={Path.workersList}
                icon={<SupervisorAccount />}
                // requireRole={RequrieRoles.User.List}
            />
            <NavButton
                text="Add worker"
                // roles={roles}
                // requireRole={RequrieRoles.User.Create}
                targetLocation={Path.addWorker}
                icon={<PersonAdd />}
            />
            <NavButton text="Account" targetLocation={Path.account} icon={<Person />} />
            <NavButton
                text="Admin Panel"
                // roles={roles}
                // requireRole={RequrieRoles.Admin}
                targetLocation={Path.admin.createCompany}
                icon={<AdminPanelSettings />}
            />
            <LogoutButton />
        </List>
    );
};

export default MenuList;
