export const Roles = {
    Admin: "Admin",
    CompanyAdmin: "CompanyAdmin",
    CompanyOwner: "CompanyOwner",
    Hr: "HR",
    Driver: "Driver",
    Dispatcher: "Dispatcher",
};

export const RequrieRoles = {
    User: {
        Create: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr],
        List: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Hr],
    },
    Location: {
        Create: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner],
        List: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner],
    },
    Delivery: {
        Create: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Dispatcher],
        List: [Roles.Admin, Roles.CompanyAdmin, Roles.CompanyOwner, Roles.Dispatcher, Roles.Driver],
    },
    Admin: [Roles.Admin],
};

const HasRole = (roles: string[], requiresRole: string[]): boolean => {
    const hasRole = roles.some((x) => requiresRole.some((y) => y === x));
    return hasRole;
};

export default HasRole;
