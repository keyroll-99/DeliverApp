import { LoadingButton } from "@mui/lab";
import Select from "components/inputs/Select";
import Snackbar from "components/snackbar/Snackbar";
import { FC, useState } from "react";
import { AssignUserToCompanyAction } from "service/companyService/CompanyServices";
import AssignUserToCompanyForm from "service/companyService/models/AssignUserToCompanyForm";
import Company from "service/companyService/models/Company";
import { UseStore } from "stores/Store";
import CreateClass from "utils/style/CreateClass";

const baseClass = "assign-user-to-company";

interface AssignUserToCompanyProps {
    companies: Company[];
}

const AssignUserToCompany: FC<AssignUserToCompanyProps> = ({ companies }) => {
    const { userStore } = UseStore();
    const [form, setForm] = useState<AssignUserToCompanyForm>({
        companyHash: "",
        userHash: userStore.getUser?.hash ?? "",
    });
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const assignUserAction = AssignUserToCompanyAction();

    const submit = async () => {
        const response = await assignUserAction.mutateAsync(form);

        if (response.isSuccess) {
            setError(null);
        } else {
            setError(response.error);
        }

        setShowSnackbar(true);
    };

    return (
        <>
            <div className={baseClass}>
                <h1 className={CreateClass(baseClass, "header")}>Assign yourself to company</h1>
                <div className={CreateClass(baseClass, "fields")}>
                    <Select
                        baseClass={baseClass}
                        label="Choose company"
                        setValue={(e) => setForm({ ...form, companyHash: e.target.value })}
                        value={form.companyHash}
                        values={companies.map((x) => ({ key: x.hash, value: x.name })) ?? []}
                    />
                </div>
                <LoadingButton
                    onClick={submit}
                    loading={assignUserAction.isLoading}
                    color="success"
                    variant="contained"
                >
                    Assign
                </LoadingButton>
            </div>
            <Snackbar
                message={
                    error ? error : `now you are assign to ${companies.find((x) => x.hash === form.companyHash)?.name}`
                }
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default AssignUserToCompany;
