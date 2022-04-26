import MenuIcon from "@mui/icons-material/Menu";
import { Drawer, IconButton } from "@mui/material";
import { useState } from "react";
import MenuList from "./MenuList";

const MobileNavbar = () => {
    const [open, setOpen] = useState(false);

    return (
        <>
            <IconButton className="navbar-toggle" onClick={() => setOpen(!open)}>
                <MenuIcon />
            </IconButton>
            <Drawer anchor="left" open={open} onBlur={() => setOpen(false)}>
                <MenuList />
            </Drawer>
        </>
    );
};

export default MobileNavbar;
