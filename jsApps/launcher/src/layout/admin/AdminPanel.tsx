import { AppBar } from "@mui/material";
import Tab from "@mui/material/Tab";
import Tabs from "@mui/material/Tabs";
import { useState } from "react";
import CreateClass from "utils/style/CreateClass";
import CompanyManagmentPanel from "./CompanyPanel";

const baseClass = "admin-panel";

const enum pages {
    CompanyManagmentPanel = 0,
}

const AdminPanel = () => {
    const [page, setPage] = useState(pages.CompanyManagmentPanel);

    return (
        <div className={baseClass}>
            <AppBar position="static" className={CreateClass(baseClass, "app-bar")} color="default">
                <Tabs textColor="primary" value={page} onChange={(e, v) => setPage(v)}>
                    <Tab label="Company managment" onClick={(e) => e.preventDefault()} />
                </Tabs>
            </AppBar>
            {page === pages.CompanyManagmentPanel && <CompanyManagmentPanel />}
        </div>
    );
};

export default AdminPanel;
