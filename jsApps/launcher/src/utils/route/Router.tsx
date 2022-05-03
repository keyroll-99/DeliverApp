import EditWorker from "layout/workers/account/editWorker/EditWorker";
import Account from "layout/workers/account/profile/Account";
import { Route, Routes } from "react-router-dom";
import Login from "../../layout/login/LoginPage";
import Menu from "../../layout/menu/Menu";
import AddWorker from "../../layout/workers/addWorker/AddWorker";
import WorkersList from "../../layout/workers/workerList/WorkersList";
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
            <Route
                path={Path.workersList}
                element={
                    <RequireAuth>
                        <WorkersList />
                    </RequireAuth>
                }
            />
            <Route
                path={Path.addWorker}
                element={
                    <RequireAuth>
                        <AddWorker />
                    </RequireAuth>
                }
            />
            <Route
                path={Path.account}
                element={
                    <RequireAuth>
                        <Account />
                    </RequireAuth>
                }
            />
            <Route
                path={Path.editWorker}
                element={
                    <RequireAuth>
                        <EditWorker />
                    </RequireAuth>
                }
            />
            <Route path={Path.login} element={<Login />} />
        </Routes>
    );
};

export default Router;
