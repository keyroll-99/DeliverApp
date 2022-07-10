import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation } from "react-query";
import { HandleApiError } from "service/_core/HandleApiError";
import { BaseResponse, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import AssignCarToDeliveryForm from "./models/AssignCarToDeliveryForm";
import AssingUserToCarForm from "./models/AssingUserToCarForm";
import CreateCarForm from "./models/CreateCarForm";
import UpdateCarForm from "./models/UpdateCarForm";

const CreateCarRequest = async (model: CreateCarForm, jwt: string): Promise<BaseResponse> => {
    const response = axios
        .post<CreateCarForm, AxiosResponse<BaseResponse>>(`${Config.serverUrl}${Endpoints.Car.Create}`, model, {
            headers: GetHeader(jwt),
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const CreateCarAction = (): MutationProcessing<CreateCarForm, BaseResponse> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((model: CreateCarForm) =>
        CreateCarRequest(model, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const UpdateCarRequest = async (model: UpdateCarForm, jwt: string): Promise<BaseResponse> => {
    const response = axios
        .put<UpdateCarForm, AxiosResponse<BaseResponse>>(`${Config.serverUrl}${Endpoints.Car.Update}`, model, {
            headers: GetHeader(jwt),
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const UpdateCarAction = (): MutationProcessing<UpdateCarForm, BaseResponse> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((form: UpdateCarForm) =>
        UpdateCarRequest(form, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const AssingUserToCarRequest = async (model: AssingUserToCarForm, jwt: string): Promise<BaseResponse> => {
    const response = await axios
        .put<AssingUserToCarForm, AxiosResponse<BaseResponse>>(
            `${Config.serverUrl}${Endpoints.Car.AssingUserToCar}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const AssingUserToCarAction = (): MutationProcessing<AssingUserToCarForm, BaseResponse> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((form: AssingUserToCarForm) =>
        AssingUserToCarRequest(form, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const AssingCarToDeliveryRequest = async (model: AssignCarToDeliveryForm, jwt: string): Promise<BaseResponse> => {
    const response = await axios
        .put<AssignCarToDeliveryForm, AxiosResponse<BaseResponse>>(
            `${Config.serverUrl}${Endpoints.Car.AssingCarToDelivery}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const AssingCarToDeliveryAction = (): MutationProcessing<AssignCarToDeliveryForm, BaseResponse> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((form: AssignCarToDeliveryForm) =>
        AssingCarToDeliveryRequest(form, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};
