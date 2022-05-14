import { Button } from "@mui/material";
import TextFieldInput from "components/inputs/TextFieldInput";
import Snackbar from "components/snackbar/Snackbar";
import CreateLocationForm from "service/location/models/CreateLocationForm";
import UpdateLocationForm from "service/location/models/UpdateLocationForm";
import CreateClass from "utils/style/CreateClass";

interface props {
    baseClass: string;
    title: string;
    submitButtonText: string;
    error: string | null;
    form: CreateLocationForm;

    setForm: (value: any) => void;
    submitForm: () => void;
}

const LocationForm = ({ baseClass, title, submitButtonText, form, error, setForm, submitForm }: props) => (
    <>
        <div className={baseClass}>
            <h1 className={CreateClass(baseClass, "title")}>{title}</h1>
            <div className={CreateClass(baseClass, "fields")}>
                <TextFieldInput
                    baseClass={baseClass}
                    label="Country"
                    onChange={(e) => setForm({ ...form, country: e.target.value })}
                    type="text"
                    error={error}
                    value={form.country}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="City"
                    onChange={(e) => setForm({ ...form, city: e.target.value })}
                    type="text"
                    error={error}
                    value={form.city}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Region"
                    onChange={(e) => setForm({ ...form, region: e.target.value })}
                    type="text"
                    error={error}
                    value={form.region}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Postal Code"
                    onChange={(e) => setForm({ ...form, postalCode: e.target.value })}
                    type="text"
                    error={error}
                    value={form.postalCode}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Street"
                    onChange={(e) => setForm({ ...form, street: e.target.value })}
                    type="text"
                    error={error}
                    value={form.street}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="No"
                    onChange={(e) => setForm({ ...form, no: e.target.value })}
                    type="text"
                    error={error}
                    value={form.no}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Email"
                    onChange={(e) => setForm({ ...form, email: e.target.value })}
                    type="text"
                    error={error}
                    value={form.email}
                />
                <TextFieldInput
                    baseClass={baseClass}
                    label="Phone number"
                    onChange={(e) => setForm({ ...form, phoneNumber: e.target.value })}
                    type="text"
                    error={error}
                    value={form.phoneNumber}
                />
            </div>
            <Button variant="contained" color="success" onClick={submitForm}>
                {submitButtonText}
            </Button>
        </div>
    </>
);

export default LocationForm;
