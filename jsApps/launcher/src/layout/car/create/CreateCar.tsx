import { LoadingButton } from "@mui/lab";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { CreateCarAction } from "service/carService/CarService";
import CreateCarForm, { GetDefaultCreateCarForm } from "service/carService/models/CreateCarForm";
import CreateClass from "utils/style/CreateClass";

const baseClass = "car-create";

const CreateCar = () => {
    const [form, setForm] = useState<CreateCarForm>(GetDefaultCreateCarForm());
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const { isLoading, mutateAsync } = CreateCarAction();

    const submitForm = async () => {
        const result = await mutateAsync(form);
        setError(result.error);
        setShowSnackbar(true);
    };

    return (
        <>
            <div className={baseClass}>
                <h1 className={CreateClass(baseClass, "header")}>Create a car</h1>
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
                message={error ? error : "Car added"}
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </>
    );
};

export default CreateCar;
