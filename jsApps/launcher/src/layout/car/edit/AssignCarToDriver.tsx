import LoadingButton from "@mui/lab/LoadingButton";
import { CircularProgress, InputLabel, MenuItem, Select } from "@mui/material";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { AssingUserToCarAction } from "service/carService/CarService";
import AssingUserToCarForm, { GetDefaultAssingUserToCarForm } from "service/carService/models/AssingUserToCarForm";
import { GetDriverAction } from "service/userService/UserService";
import CreateClass from "utils/style/CreateClass";

interface props {
    carHash: string;
    driverHash?: string;
}

const baseClass = "car-edit_assign-car-to-driver";

const AssignCarToDriver = ({ carHash, driverHash }: props) => {
    const [form, setForm] = useState<AssingUserToCarForm>(GetDefaultAssingUserToCarForm(carHash, driverHash));
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const drivers = GetDriverAction();
    const assingUserToCarAction = AssingUserToCarAction();

    const submit = async () => {
        const result = await assingUserToCarAction.mutateAsync(form);
        setError(result.error);
        setShowSnackbar(true);
    };

    if (!drivers.isSuccess) {
        return null;
    }

    return (
        <>
            <div className={baseClass}>
                {drivers.isLoading ? (
                    <CircularProgress />
                ) : (
                    <>
                        <div className={CreateClass(baseClass, "select")}>
                            <InputLabel>Select driver: </InputLabel>
                            <Select
                                id="select-driver"
                                value={form.userHash}
                                onChange={(e) => setForm({ ...form, userHash: e.target.value })}
                            >
                                {drivers.data!.map((driver) => (
                                    <MenuItem
                                        value={driver.hash}
                                        key={driver.hash}
                                    >{`${driver.name} ${driver.surname}`}</MenuItem>
                                ))}
                            </Select>
                        </div>
                        <LoadingButton variant="contained" loading={assingUserToCarAction.isLoading} onClick={submit}>
                            Assing
                        </LoadingButton>
                    </>
                )}
            </div>
            <Snackbar
                message={error ? error : "User assing to car"}
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default AssignCarToDriver;
