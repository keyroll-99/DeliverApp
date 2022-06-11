import { Refresh } from "@mui/icons-material";
import PersonRemoveIcon from "@mui/icons-material/PersonRemove";
import { LoadingButton } from "@mui/lab";
import { FireUserAction } from "service/userService/UserService";

interface props {
    userHash: string;

    refresh?: () => void;
}

const FireAction = ({ userHash, refresh }: props) => {
    const { isLoading, mutateAsync } = FireUserAction();

    const submit = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.stopPropagation();
        await mutateAsync(userHash);
    };

    return (
        <LoadingButton loading={isLoading} onClick={(e) => submit(e)}>
            <PersonRemoveIcon />
        </LoadingButton>
    );
};

export default FireAction;
