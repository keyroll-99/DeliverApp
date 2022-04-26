import PersonAddIcon from "@mui/icons-material/PersonAdd";
import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "service/userService/Roles";
import Path from "utils/route/Path";

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
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.addWorker)}>
            <PersonAddIcon />
            <p>Add worker</p>
        </ListItemButton>
    );
};

export default AddWorkerButton;
