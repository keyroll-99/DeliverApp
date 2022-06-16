import PersonRemoveIcon from "@mui/icons-material/PersonRemove";
import { LoadingButton } from "@mui/lab";
import { Portal } from "@mui/material";
import Snackbar from "components/snackbar/Snackbar";
import { FC, useState } from "react";
import { FireUserAction, GetWorkers } from "service/userService/UserService";

interface FireActionProps {
    userHash: string;
}

const FireAction: FC<FireActionProps> = ({ userHash }) => {
    const { isLoading, mutateAsync } = FireUserAction();
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const { refresh } = GetWorkers();

    const submit = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.stopPropagation();
        const result = await mutateAsync(userHash);
        if (result.isSuccess) {
            setError(null);
        } else {
            setError(result.error);
        }
        setShowSnackbar(true);
        refresh!();
    };

    return (
        <>
            <LoadingButton loading={isLoading} onClick={(e) => submit(e)}>
                <PersonRemoveIcon />
            </LoadingButton>
            <Portal container={document.querySelector("#root")}>
                <Snackbar
                    message={error ?? "Worker was fired"}
                    setIsOpen={setShowSnackbar}
                    variant={error ? "error" : "success"}
                    isOpen={showSnackbar}
                />
            </Portal>
        </>
    );
};

export default FireAction;
