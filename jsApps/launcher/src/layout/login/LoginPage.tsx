import { AccountCircle } from "@mui/icons-material";
import { Box, Container, TextField } from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { observer } from "mobx-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import LoginForm from "../../service/userService/models/LoginForm";
import { Login } from "../../service/userService/AuthenticationService";
import Path from "../../utils/route/Path";
import CreateClass from "../../utils/style/CreateClass";

const baseClass = "login";

const isValidForm = (form: LoginForm): boolean => {
    return form.username !== "" && form.password !== "";
};

const LoginPage = () => {
    const [loginForm, setLoginForm] = useState<LoginForm>({ username: "", password: "" });
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const { isLoading, mutateAsync } = Login();

    const submitForm = async () => {
        if (isValidForm(loginForm)) {
            const response = await mutateAsync(loginForm);

            if (response.isSuccess) {
                navigate(Path.home);
            } else {
                setError(response.error);
            }
        } else {
            setError("please fill in all fields");
        }
    };

    return (
        <Container fixed className={baseClass} sx={{ display: "flex" }}>
            <h1>Deliver system</h1>
            <span className={CreateClass(baseClass, "form")}>
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <AccountCircle />
                    <TextField
                        label="Login"
                        variant="standard"
                        onChange={(e) => setLoginForm({ ...loginForm, username: e.target.value })}
                        value={loginForm.username}
                        error={error ? true : false}
                        helperText={error}
                    />
                </Box>
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <AccountCircle />
                    <TextField
                        label="Password"
                        variant="standard"
                        type="password"
                        onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                        value={loginForm.password}
                        error={error ? true : false}
                        helperText={error}
                    />
                </Box>
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
