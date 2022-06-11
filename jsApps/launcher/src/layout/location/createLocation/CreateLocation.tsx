import { CircularProgress } from "@mui/material";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { CreateLocationAction } from "service/location/LocationService";
import CreateLocationForm, { GetDefaultCreateLocationForm } from "service/location/models/CreateLocationForm";
import LocationForm from "../_shared/LocationForm";

const baseClass = "add-location";

const CreateLocation = () => {
    const [error, setError] = useState<string | null>(null);
    const [form, setForm] = useState<CreateLocationForm>(GetDefaultCreateLocationForm());
    const [showSnackbar, setShowSnackbar] = useState(false);
    const { isLoading, mutateAsync } = CreateLocationAction();

    const submitForm = async () => {
        const response = await mutateAsync(form);
        if (response.isSuccess) {
            setForm(GetDefaultCreateLocationForm());
        } else {
            setError(response.error);
        }

        setShowSnackbar(true);
    };

    if (isLoading) {
        return <CircularProgress />;
    }

    return (
        <>
            <LocationForm
                baseClass={baseClass}
                title="Add new location"
                submitButtonText="Add new location"
                form={form}
                setForm={setForm}
                error={error}
                submitForm={submitForm}
            />
            <Snackbar
                message={error ? error : "New location was added"}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
                setIsOpen={setShowSnackbar}
            />
        </>
    );
};

export default CreateLocation;
