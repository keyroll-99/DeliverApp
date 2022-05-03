import { CircularProgress } from "@mui/material";
import { useParams } from "react-router-dom";
import { GetUser } from "service/userService/UserService";
import { UseStore } from "stores/Store";
import ChangePassword from "./ChangePassword";
import UpdateUser from "./UpdateUser";

export type urlParam = {
    userHash: string;
};

const EditWorker = () => {
    const { userStore } = UseStore();
    const param = useParams<keyof urlParam>() as urlParam;
    const getUser = GetUser(param.userHash);
    const user = userStore.getUser!;

    if (getUser.isLoading) {
        return <CircularProgress />;
    }

    if (!getUser.isLoading && !getUser.isSuccess) {
        return <p>oops something went wrong. please reload the page and try again</p>;
    }

    const choosedUser = getUser.data!;

    return (
        <>
            <UpdateUser
                userData={{
                    email: choosedUser.email,
                    name: choosedUser.name,
                    surname: choosedUser.surname,
                    userHash: choosedUser.hash,
                    phoneNumber: choosedUser.phoneNumber,
                }}
            />
            {user.hash === param.userHash && <ChangePassword />}
        </>
    );
};

export default EditWorker;
