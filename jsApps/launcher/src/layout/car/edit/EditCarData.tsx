import { LoadingButton } from "@mui/lab";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { UpdateCarAction } from "service/carService/CarService";
import Car from "service/carService/models/Car";
import UpdateCarForm, { GetDefaultUpdateCarForm } from "service/carService/models/UpdateCarForm";
import CreateClass from "utils/style/CreateClass";

interface props {
    car: Car;
}

const baseClass = "car-edit_data";

const EditCarData = ({ car }: props) => {
    const [form, setForm] = useState<UpdateCarForm>(GetDefaultUpdateCarForm(car));
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const { isLoading, mutateAsync } = UpdateCarAction();

    const submitForm = async () => {
        const result = await mutateAsync(form);

        setError(result.error);
        setShowSnackbar(true);
    };

    return (
        <>
            <div className={baseClass}>
                <h1 className={CreateClass(baseClass, "header")}>Update the car</h1>
                <div className={CreateClass(baseClass, "fields")}>
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Brand"
                        onChange={(e) => setForm({ ...form, brand: e.target.value })}
                        type="text"
                        value={form.brand}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Model"
                        onChange={(e) => setForm({ ...form, model: e.target.value })}
                        type="text"
                        value={form.model}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="Registration number"
                        onChange={(e) => setForm({ ...form, registrationNumber: e.target.value })}
                        type="text"
                        value={form.registrationNumber}
                    />
                    <TextFieldInput
                        baseClass={baseClass}
                        label="VIN"
                        onChange={(e) => setForm({ ...form, vin: e.target.value })}
                        type="text"
                        value={form.vin}
                    />
                </div>
                <div>
                    <LoadingButton
                        className={CreateClass(baseClass, "submit")}
                        loading={isLoading}
                        size="large"
                        onClick={submitForm}
                    >
                        submit
                    </LoadingButton>
                </div>
            </div>
            <Snackbar
                message={error ? error : "Car updated"}
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default EditCarData;
