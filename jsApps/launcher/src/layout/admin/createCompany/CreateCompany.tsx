import { LoadingButton } from "@mui/lab";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { CreateCompanyAction } from "service/companyService/CompanyServices";
import { GetDefaultCreateCompanyForm } from "service/companyService/models/CreateCompanyFormt";
import CreateClass from "utils/style/CreateClass";

const baseClass = "create-company";

const CreateCompany = () => {
    const [form, setForm] = useState(GetDefaultCreateCompanyForm());
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const createCompanyAction = CreateCompanyAction();

    const submit = async () => {
        const response = await createCompanyAction.mutateAsync(form);

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
                <h1 className={CreateClass(baseClass, "header")}>Create company</h1>
                <div className={CreateClass(baseClass, "fields")}>
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Name"
                        onChange={(e) => setForm({ ...form, name: e.target.value })}
                        value={form.name}
                        type="text"
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Email"
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        value={form.email}
                        type="email"
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Phone number"
                        onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })}
                        value={form.phoneNumber}
                        error={error}
                        type="tel"
                    />
                </div>
                <LoadingButton
                    variant="contained"
                    color="success"
                    loading={createCompanyAction.isLoading}
                    onClick={submit}
                >
                    Add company
                </LoadingButton>
            </div>
            <Snackbar
                variant={error ? "error" : "success"}
                setIsOpen={setShowSnackbar}
                message={error || "Company added"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default CreateCompany;
