import { LoadingButton } from "@mui/lab";
import { Box, Button, Container } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { InitPasswordRecoveryAction } from "service/userService/AccountService";
import InitPasswordRecoveryForm from "service/userService/models/AccountModels/InitPasswordRecoveryForm";
import { UseStore } from "stores/Store";
import Path from "utils/route/Path";
import CreateClass from "utils/style/CreateClass";
import EmailIcon from "@mui/icons-material/Email";
import PersonIcon from "@mui/icons-material/Person";

const baseClass = "password-recovery-init";

const PasswordRecoveryInit = () => {
    const [initPasswordRecoveryForm, setInitPasswordRecoveryForm] = useState<InitPasswordRecoveryForm>({
        email: "",
        username: "",
    });
    const [isSuccess, setIsSuccess] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { isLoading, mutateAsync } = InitPasswordRecoveryAction();
    const { userStore } = UseStore();

    useEffect(() => {
        if (isSuccess || userStore.getIsLogged) {
            navigate(Path.account);
        }
    }, [isSuccess, navigate, userStore.getIsLogged]);

    const submitForm = async () => {
        const response = await mutateAsync(initPasswordRecoveryForm);

        if (response.isSuccess) {
            setIsSuccess(true);
            navigate(Path.account);
        } else {
            setError(response.error);
        }
    };

    return (
        <Container fixed className={baseClass} sx={{ display: "flex" }}>
            <span className={CreateClass(baseClass, "form")}>
                <h1>Deliver system</h1>
                <TextFieldInput
                    baseClass={baseClass}
                    label="Email"
                    onChange={(e) =>
                        setInitPasswordRecoveryForm({ ...initPasswordRecoveryForm, email: e.target.value })
                    }
                    icon={<EmailIcon />}
                    error={error}
                    value={initPasswordRecoveryForm.email}
                    type="text"
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Username"
                    onChange={(e) =>
                        setInitPasswordRecoveryForm({ ...initPasswordRecoveryForm, username: e.target.value })
                    }
                    error={error}
                    type="text"
                    icon={<PersonIcon />}
                    value={initPasswordRecoveryForm.username}
                />
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <LoadingButton onClick={submitForm} loading={isLoading}>
                        Recovery
                    </LoadingButton>
                    <Button onClick={() => navigate(Path.login)}>Login</Button>
                </Box>
            </span>
        </Container>
    );
};

export default observer(PasswordRecoveryInit);
