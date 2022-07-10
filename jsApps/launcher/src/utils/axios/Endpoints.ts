const Endpoints = {
    Authentication: {
        Login: "Api/Authentication/Login",
        Refresh: "Api/Authentication/Refresh",
        Logout: "Api/Authentication/Logout",
        Permission: "Api/Authentication/Permission",
    },
    Account: {
        ChagnePassword: "Api/Account/ChangePassword",
        PasswordRecoveryInit: "Api/Account/Password-recovery/Init",
        PasswordRecoverySetNewPassword: "Api/Account/Password-recovery/Change",
        PasswordRecoveryIsValidRecoveryKey: (key: string) => `Api/Account/Password-recovery/Valid/${key}`,
    },
    User: {
        Create: "Api/User/Create",
        GetUser: (hash: string) => `Api/User/${hash}`,
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
    Car: {
        Create: "Api/Car/Create",
        Update: "Api/Car/Update",
        AssingUserToCar: "Api/Car/AssingUserToCar",
        AssingCarToDelivery: "Api/Car/AssingCarToDelivery",
        GetByHash: (hash: string) => `Api/Car/${hash}`,
        GetAll: "Api/Car",
    },
};
export default Endpoints;
