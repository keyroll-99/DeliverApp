import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "../../../service/userService/Roles";

interface props {
    roles: string[];
}

const PersonalButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr])) {
        console.log("hello");

        return null;
    }
    console.log("hello but why??");

    return (
        <ListItemButton color="" className="navbar-item" onClick={() => navigation("/test")}>
            Personel
        </ListItemButton>
    );
};

export default PersonalButton;
