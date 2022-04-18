import { ListItemButton } from "@mui/material";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import HasRole, { Roles } from "../../../service/userService/Roles";
import { useNavigate } from "react-router-dom";
import Path from "../../../utils/route/Path";

interface props {
    roles: string[];
}

const AddWorkerButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.addWorker)}>
            <PersonAddIcon />
            <p>Add worker</p>
        </ListItemButton>
    );
};

export default AddWorkerButton;
