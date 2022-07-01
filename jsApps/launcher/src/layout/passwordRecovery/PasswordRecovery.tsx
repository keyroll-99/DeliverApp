import { CircularProgress, Container } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { IsValidRecoveryKeyAction } from "service/userService/AccountService";
import InvalidRecoveryKey from "./InvalidRecoveryKey";
import PasswordRecoveryFormComponent from "./PasswordRecoveryForm";
import PasswordRecoveryForm from "./PasswordRecoveryForm";

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
    console.log(isValidKey);
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
