const Path = {
    home: "/",
    login: "/login",
    workersList: "/workers",
    addWorker: "/workers/add",
    account: "/account",
    editWorker: "/workers/:userHash",
};

export const GetPathWithParam = (url: string, value: string) => {
    return url.substring(0, url.indexOf(":")) + value;
};

export default Path;
