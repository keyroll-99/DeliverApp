import { Button, CircularProgress } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import TransferListGrid from "components/transferList/TransferListGrid";
import { transferListItemType } from "components/transferList/TransferListType";
import { useState } from "react";
import CreateUserForm from "service/userService/models/UserModels/CreateUserForm";
import UserResponse from "service/userService/models/UserModels/UserResponse";
import { GetRoles } from "service/userService/RoleService";
import { CreateUser } from "service/userService/UserService";
import { BaseResponse, MutationProcessing } from "service/_core/Models";
import CreateClass from "utils/style/CreateClass";

const baseClassName = "add-worker";

const onlyNumber = (value: string): boolean => /^[0-9]+$/.test(value);

const isValidForm = (form: CreateUserForm): string | null => {
    const isValidForm =
        form.email !== "" && form.name !== "" && form.surname !== "" && form.username !== "" && form.roleIds.length > 0;

    if (!isValidForm) {
        return "please fill in all fields";
    }

    if (form.phoneNumber && !onlyNumber(form.phoneNumber!)) {
        return "invalid phone number";
    }
    return null;
};

const submitForm = async (
    form: CreateUserForm,
    createUser: MutationProcessing<CreateUserForm, BaseResponse<UserResponse>>,
    setError: (value: string | null) => void,
    setForm: (value: CreateUserForm) => void,
    setShowSnackbar: (value: boolean) => void
) => {
    const formError = isValidForm(form);

    if (formError === null) {
        const response = await createUser.mutateAsync(form);

        if (response.isSuccess) {
            setForm({
                email: "",
                companyHash: "",
                name: "",
                roleIds: [],
                surname: "",
                username: "",
            });
            setShowSnackbar(true);
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
    const [showSnackbar, setShowSnackbar] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const getRoleRequest = GetRoles();
    const createUser = CreateUser();

    if (getRoleRequest.isLoading || createUser.isLoading) {
        return <CircularProgress />;
    }

    const availableItems = getRoleRequest
        .data!.filter((x) => form.roleIds.every((y) => y !== x.id))
        .map((x) => ({ key: x.id, value: x.name } as transferListItemType));

    const selectedItems = getRoleRequest
        .data!.filter((x) => form.roleIds.some((y) => y === x.id))
        .map((x) => ({ key: x.id, value: x.name } as transferListItemType));

    const toAvaliable = (ids: number[]) => {
        const newRoleIds = form.roleIds.filter((x) => ids.every((y) => y !== x));
        setForm({ ...form, roleIds: newRoleIds });
    };

    const toSelected = (ids: number[]) => {
        const newRoleIds = [...form.roleIds, ...ids];
        setForm({ ...form, roleIds: newRoleIds });
    };

    return (
        <>
            <span className={baseClassName}>
                <h1 className={CreateClass(baseClassName, "header")}>Add worker</h1>
                <div className={CreateClass(baseClassName, "fields")}>
                    <TextFieldInput
                        baseClass={baseClassName}
                        error={error}
                        label="Name"
                        onChange={(e) => setForm({ ...form, name: e.target.value })}
                        type="text"
                        value={form.name}
                    />
                    <TextFieldInput
                        baseClass={baseClassName}
                        error={error}
                        label="Surname"
                        onChange={(e) => setForm({ ...form, surname: e.target.value })}
                        type="text"
                        value={form.surname}
                    />
                    <TextFieldInput
                        baseClass={baseClassName}
                        error={error}
                        label="Email"
                        onChange={(e) => setForm({ ...form, email: e.target.value })}
                        type="email"
                        value={form.email}
                    />
                    <TextFieldInput
                        baseClass={baseClassName}
                        error={error}
                        label="Username"
                        onChange={(e) => setForm({ ...form, username: e.target.value })}
                        type="text"
                        value={form.username}
                    />
                    <TextFieldInput
                        baseClass={baseClassName}
                        error={error}
                        label="Phone number"
                        onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })}
                        type="tel"
                        value={form.phoneNumber}
                    />
                </div>
                <TransferListGrid
                    availableItems={availableItems}
                    selectedItems={selectedItems}
                    toAvaliable={toAvaliable}
                    toSelected={toSelected}
                    baseClass={baseClassName}
                    leftListTitle="avaliable role"
                    rightListTitle="choosed role"
                    error={error ? error : undefined}
                />
                <div>
                    <Button
                        className={CreateClass(baseClassName, "submit")}
                        size="large"
                        onClick={() => submitForm(form, createUser, setError, setForm, setShowSnackbar)}
                    >
                        submit
                    </Button>
                </div>
            </span>
            <Snackbar variant="success" setIsOpen={setShowSnackbar} message="User added" isOpen={showSnackbar} />
        </>
    );
};

export default AddWorker;
