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

    if (!isSuccess && !isLoading) {
        navigation(Path.login);
        return null;
    }

    if (isLoading) {
        return <h1>loading...</h1>;
    }

    return (
        <div className="container">
            <Navbar />
            <div className="content">{children}</div>
        </div>
    );
};

export default RequireAuth;
