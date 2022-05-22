import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";
import LocalShippingIcon from "@mui/icons-material/LocalShipping";
import HasRole, { Roles } from "service/userService/Roles";

interface props {
    roles: string[];
}

const DeliverListButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Dispatcher, Roles.Driver])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.deliveryList)}>
            <LocalShippingIcon />
            <p>delivery list</p>
        </ListItemButton>
    );
};

export default DeliverListButton;
