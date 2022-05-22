import { useState } from "react";
import { UpdateDeliveryAction } from "service/deliveryService/DeliveryService";
import Delivery from "service/deliveryService/models/Delivery";
import UpdateDeliveryFrom, { GetDefaultUpdateDeliveryForm } from "service/deliveryService/models/UpdateDeliveryForm";
import DeliverForm from "../_share/DeliverForm";

interface props {
    delivery: Delivery;
}

const baseClassName = "update-delivery";

const UpdateDelivery = ({ delivery }: props) => {
    const [form, setForm] = useState<UpdateDeliveryFrom>(GetDefaultUpdateDeliveryForm(delivery));
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const updateDelivery = UpdateDeliveryAction();

    const submitForm = async () => {
        const response = await updateDelivery.mutateAsync(form);

        if (response.isSuccess) {
            setError(null);
        } else {
            setError(response.error);
        }

        setShowSnackbar(true);
    };

    return (
        <DeliverForm
            baseClassName={baseClassName}
            error={error}
            form={form}
            isLoading={updateDelivery.isLoading}
            setForm={setForm}
            setShowSnackbar={setShowSnackbar}
            showSnackbar={showSnackbar}
            submitButtonText="Update delivery"
            submitForm={submitForm}
            successMessage="Delivery updated"
            title="Update delivery"
        />
    );
};

export default UpdateDelivery;
