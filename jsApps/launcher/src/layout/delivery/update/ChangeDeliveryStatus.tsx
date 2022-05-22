import { LoadingButton } from "@mui/lab";
import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import Snackbar from "components/snackbar/Snackbar";
import { useState } from "react";
import { ChangeDeliveryStatusAction } from "service/deliveryService/DeliveryService";
import ChangeDeliveryStatusForm, {
    GetDefaultChangeDeliveryStatusForm,
} from "service/deliveryService/models/ChangeDeliveryStatusForm";
import Delivery from "service/deliveryService/models/Delivery";
import CreateClass from "utils/style/CreateClass";

interface props {
    delivery: Delivery;
}

const baseClassName = "change-delivery-status";

const ChangeDeliveryStatus = ({ delivery }: props) => {
    const [form, setForm] = useState<ChangeDeliveryStatusForm>(
        GetDefaultChangeDeliveryStatusForm(delivery.hash, delivery.status)
    );
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const { mutateAsync, isLoading } = ChangeDeliveryStatusAction();

    const submitForm = async () => {
        const response = await mutateAsync(form);

        if (response.isSuccess) {
            setError(null);
        } else {
            setError(response.error);
        }
        setShowSnackbar(true);
    };

    return (
        <div className={baseClassName}>
            <h1 className={CreateClass(baseClassName, "title")}>Change delivery status</h1>
            <FormControl>
                <InputLabel>Status</InputLabel>
                <Select
                    value={form.newStatus}
                    onChange={(event) => setForm({ ...form, newStatus: event.target.value as number })}
                >
                    <MenuItem value={1}>New</MenuItem>
                    <MenuItem value={2}>In progress</MenuItem>
                    <MenuItem value={3}>Finished</MenuItem>
                </Select>
            </FormControl>
            <LoadingButton variant="outlined" color="success" onClick={submitForm} loading={isLoading}>
                Change status
            </LoadingButton>
            <Snackbar
                message={error ? error : "status change"}
                setIsOpen={setShowSnackbar}
                variant={error ? "error" : "success"}
                isOpen={showSnackbar}
            />
        </div>
    );
};

export default ChangeDeliveryStatus;
