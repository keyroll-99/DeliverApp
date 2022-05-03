import { CircularProgress } from "@mui/material";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Navbar from "../../layout/navbar/Navbar";
import { RefreshToken } from "../../service/userService/AuthenticationService";
import Path from "./Path";

interface props {
    children: React.ReactNode | React.ReactNode[];
}

const RequireAuth = ({ children }: props) => {
    const { isLoading, isSuccess } = RefreshToken();
    const navigation = useNavigate();

    useEffect(() => {
        if (!isSuccess && !isLoading) {
            navigation(Path.login);
        }
    }, [isSuccess, isLoading, navigation]);

    if (isLoading) {
        return <CircularProgress />;
    }
    if (!isSuccess && !isLoading) {
        return null;
    }

    return (
        <div className="container">
            <Navbar />
            <div className="content">{children}</div>
        </div>
    );
};

export default RequireAuth;
