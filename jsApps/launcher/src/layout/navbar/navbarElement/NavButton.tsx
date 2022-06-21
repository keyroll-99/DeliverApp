import { ListItemButton } from "@mui/material";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import { HasPermission, HasPermissionTo } from "service/userService/Roles";

interface props {
    text: string;
    requirePermission?: HasPermissionTo;
    targetLocation: string;
    icon?: React.ReactNode;
}

const NavButton = ({ text, requirePermission, targetLocation, icon }: props) => {
    const navigation = useNavigate();

    if (requirePermission && !HasPermission(requirePermission)) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(targetLocation)}>
            {icon}
            <p>{text}</p>
        </ListItemButton>
    );
};

export default observer(NavButton);
