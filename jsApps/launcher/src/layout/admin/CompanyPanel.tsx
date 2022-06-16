import { CircularProgress } from "@mui/material";
import { GetCompaniesAction } from "service/companyService/CompanyServices";
import AssignUserToCompany from "./assign/AssignUserToCompany";
import CreateCompany from "./createCompany/CreateCompany";

const CompanyManagmentPanel = () => {
    const { isLoading, refresh, data } = GetCompaniesAction();

    if (isLoading) {
        return <CircularProgress />;
    }

    return (
        <>
            <CreateCompany refreshCompaniesList={refresh!} />
            <AssignUserToCompany companies={data!} />
        </>
    );
};

export default CompanyManagmentPanel;
