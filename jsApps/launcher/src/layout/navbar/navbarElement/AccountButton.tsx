import { ListItemButton } from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";

const AccountButton = () => {
    const navigation = useNavigate();

    return (
        <ListItemButton className="navbar-item" onClick={() => navigation(Path.account)}>
            <PersonIcon />
            <p>Account</p>
        </ListItemButton>
    );
};

export default AccountButton;
