import { CircularProgress } from "@mui/material";
import { useParams } from "react-router-dom";
import { GetCarByHashAction } from "service/carService/CarService";
import AssignCarToDriver from "./AssignCarToDriver";
import EditCarData from "./EditCarData";

export type urlParam = {
    carHash: string;
};

const EditCar = () => {
    const param = useParams<keyof urlParam>() as urlParam;
    const getCarAction = GetCarByHashAction(param.carHash);

    if (getCarAction.isLoading) {
        return <CircularProgress />;
    }

    console.log(getCarAction);

    return (
        <>
            <EditCarData car={getCarAction.data!} />
            <AssignCarToDriver carHash={param.carHash} driverHash={getCarAction.data?.driver?.hash} />
        </>
    );
};

export default EditCar;
