import { Button } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import ChangePasswordForm from "service/userService/models/ChangePasswordForm";
import { ChangePasswordAction } from "service/userService/UserService";
import CreateClass from "utils/style/CreateClass";

const baseClass = "change-password";

const ChangePassword = () => {
    const [form, setForm] = useState<ChangePasswordForm>({ oldPassword: "", password: "" });
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const changePasswordAction = ChangePasswordAction();

    const submitForm = async () => {
        const result = await changePasswordAction.mutateAsync(form);

        if (result.isSuccess) {
            setForm({ password: "", oldPassword: "" });
            setError(null);
            setShowSnackbar(true);
        } else {
            setError(result.error);
        }
    };

    return (
        <>
            <div className={baseClass}>
                <h1>Change password</h1>
                <div className={CreateClass(baseClass, "fields")}>
                    <TextFieldInput
                        baseClass={baseClass}
                        error={error}
                        label={"current password"}
                        onChange={(e) => setForm({ ...form, oldPassword: e.target.value })}
                        type="password"
                        value={form.oldPassword}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        error={error}
                        label={"new password"}
                        onChange={(e) => setForm({ ...form, password: e.target.value })}
                        type="password"
                        value={form.password}
                    />
                </div>
                <Button
                    variant="contained"
                    color="success"
                    className={CreateClass(baseClass, "submit")}
                    onClick={submitForm}
                >
                    Change password
                </Button>
            </div>
            <Snackbar variant="success" setIsOpen={setShowSnackbar} message="Password change" isOpen={showSnackbar} />
        </>
    );
};

export default ChangePassword;
