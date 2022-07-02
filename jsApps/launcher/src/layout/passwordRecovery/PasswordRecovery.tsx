import { CircularProgress } from "@mui/material";
import { useParams } from "react-router-dom";
import { IsValidRecoveryKeyAction } from "service/userService/AccountService";
import InvalidRecoveryKey from "./InvalidRecoveryKey";
import PasswordRecoveryFormComponent from "./PasswordRecoveryFormComponent";

export type urlParam = {
    recoveryKey: string;
};

const baseClass = "password-recovery";

const PasswordRecovery = () => {
    const param = useParams<keyof urlParam>() as urlParam;
    const isValidKey = IsValidRecoveryKeyAction(param.recoveryKey);

    if (isValidKey.isLoading) {
        return <CircularProgress />;
    }

    return (
        <div className={baseClass}>
            {isValidKey.data ? (
                <PasswordRecoveryFormComponent recoveryKey={param.recoveryKey} />
            ) : (
                <InvalidRecoveryKey />
            )}
        </div>
    );
};

export default PasswordRecovery;
