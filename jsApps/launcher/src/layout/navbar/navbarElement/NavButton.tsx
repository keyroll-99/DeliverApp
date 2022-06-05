import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole from "service/userService/Roles";

interface props {
    text: string;
    roles: string[];
    requireRole?: string[];
    targetLocation: string;
    icon?: React.ReactNode;
}

const NavButton = ({ text, roles, requireRole, targetLocation, icon }: props) => {
    const navigation = useNavigate();

    if (requireRole && !HasRole(roles, requireRole)) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(targetLocation)}>
            {icon}
            <p>{text}</p>
        </ListItemButton>
    );
};

export default NavButton;
