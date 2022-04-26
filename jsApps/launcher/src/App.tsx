import { CircularProgress } from "@mui/material";
import { observer } from "mobx-react";
import { LoadConfig } from "utils/_core/Config";
import Router from "./utils/route/Router";

const App = () => {
    const { isLoading } = LoadConfig();

    if (isLoading) {
        return <CircularProgress />;
    }

    return <Router />;
};

export default observer(App);
