import { observer } from "mobx-react";
import { RefreshToken } from "./service/userService/UserService";
import Router from "./utils/Route/Router";

const App = () => {
    const { isLoading } = RefreshToken();

    if (isLoading) {
        return <h1>loading...</h1>;
    }

    return <Router />;
};

export default observer(App);
