import { useParams } from "react-router-dom";

export type urlParam = {
    recoveryKey: string;
};

const PasswordRecovery = () => {
    const param = useParams<keyof urlParam>() as urlParam;

    return <h1>test recovery {param.recoveryKey}</h1>;
};

export default PasswordRecovery;
