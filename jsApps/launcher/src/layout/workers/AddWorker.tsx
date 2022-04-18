import { CircularProgress } from "@mui/material";
import { useState } from "react";
import CreateUserForm from "../../service/userService/models/CreateUserForm";
import LoginForm from "../../service/userService/models/LoginForm";
import UserResponse from "../../service/userService/models/UserResponse";
import { GetRoles } from "../../service/userService/RoleService";
import { CreateUser } from "../../service/userService/UserService";
import { BaseResponse, MutationProcessing } from "../../service/_core/Models";
import { UseStore } from "../../stores/Store";

const baseClassName = "add-worker";

const isValidForm = (from: CreateUserForm): string | undefined => {
    return undefined;
};

const submitForm = async (
    form: CreateUserForm,
    createUser: MutationProcessing<CreateUserForm, BaseResponse<UserResponse>>,
    setError: (value: string | undefined) => void
) => {
    const formError = isValidForm(form);

    if (formError === "") {
        const response = await createUser.mutateAsync(form);

        if (response.isSuccess) {
            console.log(response.data);
        } else {
            setError(response.error);
        }
    } else {
        setError(formError);
    }
};

const AddWorker = () => {
    const [form, setForm] = useState<CreateUserForm>({
        email: "",
        companyHash: "",
        name: "",
        roleIds: [],
        surname: "",
        username: "",
    });
    const [error, setError] = useState<string | undefined>();
    const getRoleRequest = GetRoles();
    const createUser = CreateUser();

    if (getRoleRequest.isLoading) {
        return <CircularProgress />;
    }

    return <div className={baseClassName}></div>;
};

export default AddWorker;
