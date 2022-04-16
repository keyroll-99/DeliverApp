import MenuIcon from "@mui/icons-material/Menu";
import { Drawer, IconButton, List, ListItem, ListItemText } from "@mui/material";
import { useState } from "react";
import { Navigate } from "react-router-dom";
import { UseStore } from "../../stores/Store";
import PersonalButton from "./navbarElement/PersonalButton";

const MobileNavbar = () => {
    const [open, setOpen] = useState(false);
    const { userStore } = UseStore();

    return (
        <>
            <IconButton onClick={() => setOpen(!open)}>
                <MenuIcon />
            </IconButton>
            <Drawer anchor="left" open={open} onBlur={() => setOpen(false)}>
                <List className="navbar">
                    <PersonalButton roles={userStore.getUser!.roles} />
                </List>
            </Drawer>
        </>
    );
};

export default MobileNavbar;
