import { ListItemButton } from "@mui/material";
import ExitToAppIcon from "@mui/icons-material/ExitToApp";
import { Logout } from "service/userService/AuthenticationService";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";

const LogoutButton = () => {
    const { mutateAsync } = Logout();
    const navigation = useNavigate();

    const logout = async () => {
        const response = await mutateAsync(undefined);
        if (response.isSuccess) {
            navigation(Path.login);
        }
    };

    return (
        <ListItemButton className="navbar-item" onClick={logout}>
            <ExitToAppIcon />
            <p>Logout</p>
        </ListItemButton>
    );
};

export default LogoutButton;
