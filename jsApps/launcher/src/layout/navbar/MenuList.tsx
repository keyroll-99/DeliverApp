import { List } from "@mui/material";
import { UseStore } from "stores/Store";
import AddWorkerButton from "./navbarElement/AddWorkerButton";
import AccountButton from "./navbarElement/AccountButton";
import WorkerButton from "./navbarElement/WorkerButton";
import LogoutButton from "./navbarElement/LogoutButton";
import LocationListButton from "./navbarElement/LocationListButton";
import LocationAddButton from "./navbarElement/LocationAddButton";
import DeliverListButton from "./navbarElement/DeliveryListButton";
import DeliveryCreateButton from "./navbarElement/DeliveryCreateButton";

const MenuList = () => {
    const { userStore } = UseStore();

    const roles = userStore.getUser!.roles;
    return (
        <List className="navbar-list">
            <DeliverListButton roles={roles} />
            <DeliveryCreateButton roles={roles} />
            <LocationListButton roles={roles} />
            <LocationAddButton roles={roles} />
            <WorkerButton roles={roles} />
            <AddWorkerButton roles={roles} />
            <AccountButton />
            <LogoutButton />
        </List>
    );
};

export default MenuList;
