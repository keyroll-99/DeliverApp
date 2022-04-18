import { ListItemButton } from "@mui/material";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import HasRole, { Roles } from "../../../service/userService/Roles";
import { useNavigate } from "react-router-dom";

interface props {
    roles: string[];
}

const AddWorkerButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr])) {
        return null;
    }

    return (
        // tmp solition for not working import on CI
        <ListItemButton className="navbar-item" onClick={() => navigation("/workers/add")}>
            <PersonAddIcon />
            <p>Add worker</p>
        </ListItemButton>
    );
};

export default AddWorkerButton;
