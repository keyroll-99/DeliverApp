import { AccountCircle } from "@mui/icons-material";
import { Box, Button, Container, TextField } from "@mui/material";
import { observer } from "mobx-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import LoginForm from "../../service/userService/models/LoginForm";
import { Login } from "../../service/userService/UserService";
import Path from "../../utils/Route/Path";
import CreateClass from "../../utils/style/CreateClass";

const baseClass = "login";

const isValidForm = (form: LoginForm): boolean => {
    return form.Username !== "" && form.Password !== "";
};

const LoginPage = () => {
    const [loginForm, setLoginForm] = useState<LoginForm>({ Username: "", Password: "" });
    const navigate = useNavigate();

    const onSuccess = () => {
        navigate(Path.home);
    };
    const { isLoading, mutate } = Login(onSuccess);

    const submitForm = async () => {
        if (isValidForm(loginForm)) {
            mutate(loginForm);
        }
    };

    if (isLoading) {
        return <h1>loading...</h1>;
    }

    return (
        <Container fixed className={baseClass} sx={{ display: "flex" }}>
            <h1>Deliver system</h1>
            <span className={CreateClass(baseClass, "form")}>
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <AccountCircle />
                    <TextField
                        label="Login"
                        variant="standard"
                        onChange={(e) => setLoginForm({ ...loginForm, Username: e.target.value })}
                        value={loginForm.Username}
                    />
                </Box>
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <AccountCircle />
                    <TextField
                        label="Password"
                        variant="standard"
                        type="password"
                        onChange={(e) => setLoginForm({ ...loginForm, Password: e.target.value })}
                        value={loginForm.Password}
                    />
                </Box>
                <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
                    <Button onClick={submitForm}>Login</Button>
                </Box>
            </span>
        </Container>
    );
};

export default observer(LoginPage);
