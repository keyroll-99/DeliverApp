import { AccountCircle } from "@mui/icons-material";
import LockIcon from "@mui/icons-material/Lock";
import { LoadingButton } from "@mui/lab";
import { Box, Container } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Login } from "service/userService/AuthenticationService";
import LoginForm from "service/userService/models/AuthModels/LoginForm";
import { UseStore } from "stores/Store";
import Path from "utils/route/Path";
import CreateClass from "utils/style/CreateClass";

const baseClass = "login";

const isValidForm = (form: LoginForm): boolean => {
    return form.username !== "" && form.password !== "";
};

const LoginPage = () => {
    const [loginForm, setLoginForm] = useState<LoginForm>({ username: "", password: "" });
    const [isSuccess, setIsSuccess] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { isLoading, mutateAsync } = Login();
    const { userStore } = UseStore();

    useEffect(() => {
        if (isSuccess || userStore.getIsLogged) {
            navigate(Path.account);
        }
    }, [isSuccess, navigate, userStore.getIsLogged]);

    const submitForm = async () => {
        if (isValidForm(loginForm)) {
            const response = await mutateAsync(loginForm);

            if (response.isSuccess) {
                setIsSuccess(true);
                navigate(Path.account);
            } else {
                setError(response.error);
            }
        } else {
            setError("please fill in all fields");
        }
    };

    return (
        <Container fixed className={baseClass} sx={{ display: "flex" }}>
            <span className={CreateClass(baseClass, "form")}>
                <h1>Deliver system</h1>
                <TextFieldInput
                    baseClass={baseClass}
                    label="Login"
                    onChange={(e) => setLoginForm({ ...loginForm, username: e.target.value })}
                    error={error}
                    icon={<AccountCircle />}
                    value={loginForm.username}
                    type="text"
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Password"
                    onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                    error={error}
                    type="password"
                    value={loginForm.password}
                    icon={<LockIcon />}
                />
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <LoadingButton onClick={submitForm} loading={isLoading}>
                        Login
                    </LoadingButton>
                </Box>
            </span>
        </Container>
    );
};

export default observer(LoginPage);
