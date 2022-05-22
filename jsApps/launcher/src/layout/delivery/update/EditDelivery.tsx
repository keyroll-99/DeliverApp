import { CircularProgress } from "@mui/material";
import { useParams } from "react-router-dom";
import { GetDeliveryByHash } from "service/deliveryService/DeliveryService";
import ChangeDeliveryStatus from "./ChangeDeliveryStatus";
import UpdateDelivery from "./UpdateDelivery";

export type urlParam = {
    deliveryHash: string;
};

const EditDelivery = () => {
    const param = useParams<keyof urlParam>() as urlParam;
    const { isLoading, isSuccess, data } = GetDeliveryByHash(param.deliveryHash);

    if (isLoading) {
        return <CircularProgress />;
    }

    if (!isSuccess) {
        return <p>please refresh page</p>;
    }

    return (
        <>
            <UpdateDelivery delivery={data!} />
            <ChangeDeliveryStatus delivery={data!} />
        </>
    );
};

export default EditDelivery;
