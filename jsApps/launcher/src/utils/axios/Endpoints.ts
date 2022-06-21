const Endpoints = {
    Authentication: {
        Login: "Api/Authentication/Login",
        Refresh: "Api/Authentication/Refresh",
        Logout: "Api/Authentication/Logout",
        Permission: "Api/Authentication/Permission",
    },
    User: {
        Create: "Api/User/Create",
        GetUser: (hash: string) => `Api/User/${hash}`,
        ChagnePassword: "Api/User/ChangePassword",
        UpdateUser: "Api/User/Update",
        UserList: "Api/User/List",
        Fire: (hash: string) => `Api/User/Fire/${hash}`,
    },
    Company: {
        Create: "Api/Company/Create",
        List: "Api/Company/List",
        AssingUserToCompany: "Api/Company/Assing",
    },
    Role: {
        GetAll: "Api/Role",
    },
    Location: {
        Create: "Api/Location",
        GetList: "Api/Location/List",
        Update: "Api/Location",
        GetByHash: (hash: string) => `Api/Location/${hash}`,
    },
    Delivery: {
        GetList: "Api/Delivery/List",
        GetByHash: (hash: string) => `Api/Delivery/${hash}`,
        Create: "Api/Delivery",
        ChangeStatus: "Api/Delivery/Status",
        Update: "Api/Delivery",
    },
};
export default Endpoints;
