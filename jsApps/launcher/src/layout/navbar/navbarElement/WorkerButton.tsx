import SupervisorAccountIcon from "@mui/icons-material/SupervisorAccount";
import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "service/userService/Roles";
import Path from "utils/route/Path";

interface props {
    roles: string[];
}

const WorkerButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.workersList)}>
            <SupervisorAccountIcon />
            <p>Workers</p>
        </ListItemButton>
    );
};

export default WorkerButton;
