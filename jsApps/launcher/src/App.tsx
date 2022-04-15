import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { useQueries, useQuery, useQueryClient } from "react-query";
import { RefreshToken } from "./service/userService/UserService";
import { UseStore } from "./stores/Store";
import Router from "./utils/Route/Router";

const App = () => {
    const { userStore } = UseStore();
    const { isLoading, error } = RefreshToken();

    if (isLoading) {
        return <h1>loading...</h1>;
    }

    return <Router />;
};

export default observer(App);
