import {
    Add,
    AddLocationAlt,
    LocalShipping,
    LocationOn,
    Person,
    PersonAdd,
    SupervisorAccount,
    AdminPanelSettings,
} from "@mui/icons-material";
import { List } from "@mui/material";
import { RequrieRoles } from "service/userService/Roles";
import { UseStore } from "stores/Store";
import Path from "utils/route/Path";
import LogoutButton from "./navbarElement/LogoutButton";
import NavButton from "./navbarElement/NavButton";

const MenuList = () => {
    const { userStore } = UseStore();

    const roles = userStore.getUser!.roles;
    return (
        <List className="navbar-list">
            <NavButton
                text="Delivery list"
                roles={roles}
                targetLocation={Path.deliveryList}
                icon={<LocalShipping />}
                requireRole={RequrieRoles.Delivery.List}
            />
            <NavButton
                text="Add delivery"
                roles={roles}
                targetLocation={Path.deliveryCreate}
                icon={<Add />}
                requireRole={RequrieRoles.Delivery.Create}
            />
            <NavButton
                text="Locations"
                roles={roles}
                targetLocation={Path.locationList}
                icon={<LocationOn />}
                requireRole={RequrieRoles.Location.List}
            />
            <NavButton
                text="Add location"
                roles={roles}
                targetLocation={Path.locationAdd}
                icon={<AddLocationAlt />}
                requireRole={RequrieRoles.Location.Create}
            />
            <NavButton
                text="Workers"
                roles={roles}
                targetLocation={Path.workersList}
                icon={<SupervisorAccount />}
                requireRole={RequrieRoles.User.List}
            />
            <NavButton
                text="Add worker"
                roles={roles}
                requireRole={RequrieRoles.User.Create}
                targetLocation={Path.addWorker}
                icon={<PersonAdd />}
            />
            <NavButton text="Account" roles={roles} targetLocation={Path.account} icon={<Person />} />
            <NavButton
                text="Admin Panel"
                roles={roles}
                targetLocation={Path.admin.createCompany}
                icon={<AdminPanelSettings />}
            />
            <LogoutButton />
        </List>
    );
};

export default MenuList;
