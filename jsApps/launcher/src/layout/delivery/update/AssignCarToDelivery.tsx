import { LoadingButton } from "@mui/lab";
import { CircularProgress, InputLabel, MenuItem, Select } from "@mui/material";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { AssingCarToDeliveryAction, GetCarsListAction } from "service/carService/CarService";
import AssignCarToDeliveryForm, {
    GetDefaultAssignCarToDeliveryForm,
} from "service/carService/models/AssignCarToDeliveryForm";
import CreateClass from "utils/style/CreateClass";

interface props {
    deliveryHash: string;
    carHash?: string;
}

const baseClass = "assign-car-to-delivery";

const AssignCarToDelivery = ({ deliveryHash, carHash }: props) => {
    const [form, setForm] = useState<AssignCarToDeliveryForm>(GetDefaultAssignCarToDeliveryForm(deliveryHash, carHash));
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const assignCarToDelivery = AssingCarToDeliveryAction();
    const carsAction = GetCarsListAction();

    const submit = async () => {
        const response = await assignCarToDelivery.mutateAsync(form);
        setError(response.error);
        setShowSnackbar(true);
    };

    if (!carsAction.isSuccess) {
        return null;
    }

    return (
        <>
            <div className={baseClass}>
                <h1>Assign car to delivery</h1>
                {carsAction.isLoading ? (
                    <CircularProgress />
                ) : (
                    <>
                        <div className={CreateClass(baseClass, "select")}>
                            <InputLabel>Select car for delivery</InputLabel>
                            <Select
                                value={form.carHash}
                                id="select-car"
                                onChange={(e) => setForm({ ...form, carHash: e.target.value })}
                            >
                                {carsAction.data?.map((car) => (
                                    <MenuItem
                                        value={car.hash}
                                        key={car.hash}
                                    >{`${car.brand} ${car.model} ${car.registrationNumber}`}</MenuItem>
                                ))}
                            </Select>
                        </div>
                        <LoadingButton variant="outlined" color="success" onClick={submit} size="large">
                            Assign
                        </LoadingButton>
                    </>
                )}
            </div>
            <Snackbar
                message={error ? error : "Car assign to delivery"}
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default AssignCarToDelivery;
