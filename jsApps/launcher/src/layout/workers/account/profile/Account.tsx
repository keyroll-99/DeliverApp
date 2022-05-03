import { Button, CircularProgress, Divider } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { GetUser } from "service/userService/UserService";
import { UseStore } from "stores/Store";
import Path, { GetPathWithParam } from "utils/route/Path";
import CreateClass from "utils/style/CreateClass";

const baseClass = "account";

const Account = () => {
    const { userStore } = UseStore();
    const { isLoading, data, isSuccess } = GetUser(userStore.getUser!.hash);
    const navigation = useNavigate();

    const user = data!;

    if (isLoading) {
        return <CircularProgress />;
    }

    if (!isSuccess) {
        return <p>oops something went wrong. please reload the page and try again</p>;
    }

    return (
        <div className={baseClass}>
            <h1 className={CreateClass(baseClass, "title")}>Account settings</h1>
            <div className={CreateClass(baseClass, "item")}>
                <p>Name: </p>
                <p>
                    {user.name} {user.surname}
                </p>
            </div>
            <Divider />
            <div className={CreateClass(baseClass, "item")}>
                <p>Username: </p>
                <p>{user.username}</p>
            </div>
            <Divider />
            <div className={CreateClass(baseClass, "item")}>
                <p>Company name: </p>
                <p>{user.companyName}</p>
            </div>
            <Divider />
            <div className={CreateClass(baseClass, "item")}>
                <p>Roles: </p>
                <ul className={CreateClass(baseClass, "list")}>
                    {user.roles.map((x) => (
                        <li key={x}>{x}</li>
                    ))}
                </ul>
            </div>
            <Divider />
            <div className={CreateClass(baseClass, "item")}>
                <Button
                    onClick={() => navigation(GetPathWithParam(Path.editWorker, userStore.getUser!.hash))}
                    variant="contained"
                >
                    Change data
                </Button>
            </div>
        </div>
    );
};

export default Account;
