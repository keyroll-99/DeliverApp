import AddIcon from "@mui/icons-material/Add";
import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "service/userService/Roles";
import Path from "utils/route/Path";

interface props {
    roles: string[];
}

const DeliveryCreateButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Dispatcher])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.deliveryCreate)}>
            <AddIcon />
            <p>Add delivery</p>
        </ListItemButton>
    );
};

export default DeliveryCreateButton;
