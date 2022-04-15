import { Routes, Route } from "react-router-dom";
import Login from "../../layout/login/LoginPage";
import Menu from "../../layout/menu/Menu";
import Path from "./Path";
import RequireAuth from "./RequireAuth";

const Router = () => {
    return (
        <Routes>
            <Route
                path={Path.home}
                element={
                    <RequireAuth>
                        <Menu />
                    </RequireAuth>
                }
            />
            <Route path={Path.login} element={<Login />} />
        </Routes>
    );
};

export default Router;
