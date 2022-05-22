import { useState } from "react";
import { CreateDeliveryAction } from "service/deliveryService/DeliveryService";
import CreateDeliveryForm, { GetDefaultCreateDeliveryForm } from "service/deliveryService/models/CreateDeliveryForm";
import DeliverForm from "../_share/DeliverForm";

const baseClassName = "create-delivery";

const CreateDelivery = () => {
    const [form, setForm] = useState<CreateDeliveryForm>(GetDefaultCreateDeliveryForm());
    const [error, setError] = useState<string | null>(null);
    const [showSnackbar, setShowSnackbar] = useState(false);
    const createDelivery = CreateDeliveryAction();

    const submitForm = async () => {
        const response = await createDelivery.mutateAsync(form);

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
            isLoading={createDelivery.isLoading}
            setForm={setForm}
            setShowSnackbar={setShowSnackbar}
            showSnackbar={showSnackbar}
            submitButtonText="Add delivery"
            submitForm={submitForm}
            successMessage={"Delivery has been added"}
            title={"add delivery"}
        />
    );
};

export default CreateDelivery;
