import LocationAltIcon from "@mui/icons-material/AddLocationAlt";
import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "service/userService/Roles";
import Path from "utils/route/Path";

interface props {
    roles: string[];
}

const LocationAddButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.locationAdd)}>
            <LocationAltIcon />
            <p>Add location</p>
        </ListItemButton>
    );
};

export default LocationAddButton;
