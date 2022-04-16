import { Navigate } from "react-router-dom";
import Navbar from "../../layout/navbar/Navbar";
import { UseStore } from "../../stores/Store";
import Path from "./Path";

interface props {
    children: React.ReactNode | React.ReactNode[];
}

const RequireAuth = ({ children }: props) => {
    const { userStore } = UseStore();

    if (!userStore.getIsLogged) {
        Navigate({ to: Path.login, replace: true });
        return <></>;
    }

    return (
        <div className="container">
            <Navbar />
            {children}
        </div>
    );
};

export default RequireAuth;
