const Path = {
    home: "/",
    login: "/login",
    workersList: "/workers",
    addWorker: "/workers/add",
    account: "/account",
    editWorker: "/workers/:userHash",
    locationAdd: "/location/add",
    locationList: "/location/list",
    locationUpdate: "/location/:locationHash",
    deliveryList: "/delivery/list",
    deliveryCreate: "/delivery/create",
    deliveryUpdate: "/delivery/:deliveryHash",
};

export const GetPathWithParam = (url: string, value: string) => {
    return url.substring(0, url.indexOf(":")) + value;
};

export default Path;
