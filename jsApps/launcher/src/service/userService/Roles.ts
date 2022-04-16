export const Roles = {
    Admin: "Admin",
    CompanyAdmin: "CompanyAdmin",
    CompanyOwner: "CompanyOwner",
    Hr: "HR",
    Driver: "Driver",
};

const HasRole = (roles: string[], requiresRole: string[]): boolean => {
    const hasRole = roles.some((x) => requiresRole.some((y) => y === x));
    return hasRole;
};

export default HasRole;
