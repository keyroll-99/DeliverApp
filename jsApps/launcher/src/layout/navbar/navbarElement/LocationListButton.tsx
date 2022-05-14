import LocationOnIcon from "@mui/icons-material/LocationOn";
import { ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import HasRole, { Roles } from "service/userService/Roles";
import Path from "utils/route/Path";

interface props {
    roles: string[];
}

const LocationListButton = ({ roles }: props) => {
    const navigation = useNavigate();

    if (!HasRole(roles, [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner])) {
        return null;
    }

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.locationList)}>
            <LocationOnIcon />
            <p>Locations</p>
        </ListItemButton>
    );
};

export default LocationListButton;
