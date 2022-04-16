import { List } from "@mui/material";
import { UseStore } from "../../stores/Store";
import PersonalButton from "./navbarElement/PersonalButton";

const DesktopNavbar = () => {
    const { userStore } = UseStore();

    return (
        <div className="navbar">
            <List>
                <PersonalButton roles={userStore.getUser!.roles} />
            </List>
        </div>
    );
};

export default DesktopNavbar;
