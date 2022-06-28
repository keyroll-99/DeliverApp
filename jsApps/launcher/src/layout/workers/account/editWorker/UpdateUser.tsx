import { Button } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import UpdateUserForm from "service/userService/models/UserModels/UpdateUserForm";
import CreateClass from "utils/style/CreateClass";
import { UpdateUserAction } from "service/userService/UserService";

interface props {
    userData: UpdateUserForm;
}

const baseClass = "change-user-data";

const UpdateUser = ({ userData }: props) => {
    const [form, setForm] = useState<UpdateUserForm>(userData);
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const updateUserAction = UpdateUserAction();

    const submitForm = async () => {
        const result = await updateUserAction.mutateAsync(form);

        if (!result.isSuccess) {
            setError(result.error);
        }
        setShowSnackbar(true);
    };

    return (
        <>
            <div className={baseClass}>
                <h1 className={CreateClass(baseClass, "title")}>Change data</h1>
                <div className={CreateClass(baseClass, "fields")}>
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Name"
                        onChange={(e) => setForm({ ...form, name: e.target.value })}
                        type="text"
                        value={form.name}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Surname"
                        onChange={(e) => setForm({ ...form, surname: e.target.value })}
                        type="text"
                        value={form.surname}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Email"
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        type="email"
                        value={form.email}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Phone number"
                        onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })}
                        type="tel"
                        value={form.phoneNumber}
                    />
                </div>
                <Button variant="contained" color="success" onClick={submitForm}>
                    Change data
                </Button>
            </div>
            <Snackbar
                message={error ? error : "The change of data was successful"}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
                setIsOpen={setShowSnackbar}
            />
        </>
    );
};

export default UpdateUser;
