import { List } from "@mui/material";
import { UseStore } from "stores/Store";
import AddWorkerButton from "./navbarElement/AddWorkerButton";
import AccountButton from "./navbarElement/AccountButton";
import WorkerButton from "./navbarElement/WorkerButton";

const MenuList = () => {
    const { userStore } = UseStore();

    const roles = userStore.getUser!.roles;
    return (
        <List className="navbar-list">
            <WorkerButton roles={roles} />
            <AddWorkerButton roles={roles} />
            <AccountButton />
        </List>
    );
};

export default MenuList;
