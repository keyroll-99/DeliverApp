const Endpoints = {
    Authentication: {
        Login: "Api/Authentication/Login",
        Refresh: "Api/Authentication/Refresh",
    },
    User: {
        Create: "Api/User/Create",
        GetUser: (hash: string) => `Api/User/${hash}`,
        ChagnePassword: "Api/User/ChangePassword",
        UpdateUser: "Api/User/Update",
    },
    Company: {
        Workers: "Api/Company/Workers",
    },
    Role: {
        GetAll: "Api/Role",
    },
};
export default Endpoints;
