import { Alert, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";

const InvalidRecoveryKey = () => {
    const navigation = useNavigate();

    return (
        <div className="invalid-recovery-key">
            <Alert variant="filled" severity="warning">
                Your recovery link is invalid
            </Alert>
            <Button onClick={() => navigation(Path.login)} variant="contained">
                Back to login page
            </Button>
        </div>
    );
};

export default InvalidRecoveryKey;
