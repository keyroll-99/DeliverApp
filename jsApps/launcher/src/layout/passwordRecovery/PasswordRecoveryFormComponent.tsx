import TextFieldInput from "components/inputs/TextFieldInput";
import { useState } from "react";
import { PasswordRecoveryAction } from "service/userService/AccountService";
import PasswordRecoveryForm from "service/userService/models/AccountModels/PasswordRecoveryForm";
import LockIcon from "@mui/icons-material/Lock";
import { Alert, Button } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";

const baseClass = "password-recovery-form";

interface props {
    recoveryKey: string;
}

const PasswordRecoveryFormComponent = ({ recoveryKey }: props) => {
    const { isLoading, mutateAsync } = PasswordRecoveryAction();
    const [form, setForm] = useState<PasswordRecoveryForm>({ newPassword: "", recoveryKey: recoveryKey });
    const [error, setError] = useState<boolean>(false);
    const navigation = useNavigate();
    const [message, setMessage] = useState<string | null>(null);

    const submitForm = async () => {
        const response = await mutateAsync(form);
        setError(!response.isSuccess);
        if (response.isSuccess) {
            setMessage("Password changed");
        } else {
            setMessage(response.error);
        }
    };

    return (
        <>
            <TextFieldInput
                baseClass={baseClass}
                label="New password"
                onChange={(e) => setForm({ ...form, newPassword: e.target.value })}
                type="password"
                icon={<LockIcon />}
            />
            <LoadingButton loading={isLoading} variant="outlined" onClick={submitForm} color="success">
                Set new password
            </LoadingButton>
            {message && (
                <Alert variant="filled" severity={error ? "error" : "success"}>
                    {message}
                </Alert>
            )}
            {message && !error && (
                <Button variant="outlined" onClick={() => navigation(Path.login)}>
                    Back to login page
                </Button>
            )}
        </>
    );
};

export default PasswordRecoveryFormComponent;
