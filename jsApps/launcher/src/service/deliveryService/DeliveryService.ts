import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { HandleApiError } from "service/_core/HandleApiError";
import { BaseResponse, FetchProcessing, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import ChangeDeliveryStatusForm from "./models/ChangeDeliveryStatusForm";
import CreateDeliveryFrom from "./models/CreateDeliveryForm";
import Delivery from "./models/Delivery";
import UpdateDeliveryFrom from "./models/UpdateDeliveryForm";

const GetDeliveryListRequest = async (jwt: string): Promise<BaseResponse<Delivery[]>> => {
    const response = await axios
        .get<null, AxiosResponse<BaseResponse<Delivery[]>>>(`${Config.serverUrl}${Endpoints.Delivery.GetList}`, {
            headers: GetHeader(jwt),
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Delivery[]>(error));

    return response;
};

export const GetDeliveryList = (): FetchProcessing<Delivery[]> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery("fetch-delivery-list", () => GetDeliveryListRequest(userStore.getUser!.jwt));

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};

const CreateDeliveryRequest = async (model: CreateDeliveryFrom, jwt: string): Promise<BaseResponse<Delivery>> => {
    const response = await axios
        .post<CreateDeliveryFrom, AxiosResponse<BaseResponse<Delivery>>>(
            `${Config.serverUrl}${Endpoints.Delivery.Create}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Delivery>(error));

    return response;
};

export const CreateDeliveryAction = (): MutationProcessing<CreateDeliveryFrom, BaseResponse<Delivery>> => {
    const { userStore } = UseStore();

    const { mutateAsync, isLoading } = useMutation((model: CreateDeliveryFrom) =>
        CreateDeliveryRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const ChangeDeliveryStatusRequest = async (
    model: ChangeDeliveryStatusForm,
    jwt: string
): Promise<BaseResponse<Delivery>> => {
    const response = await axios
        .put<ChangeDeliveryStatusForm, AxiosResponse<BaseResponse<Delivery>>>(
            `${Config.serverUrl}${Endpoints.Delivery.ChangeStatus}`,
            model,
            {
                headers: GetHeader(jwt),
            }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Delivery>(error));

    return response;
};

export const ChangeDeliveryStatusAction = (): MutationProcessing<ChangeDeliveryStatusForm, BaseResponse<Delivery>> => {
    const { userStore } = UseStore();

    const { mutateAsync, isLoading } = useMutation((model: ChangeDeliveryStatusForm) =>
        ChangeDeliveryStatusRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const UpdateDeliveryRequest = async (model: UpdateDeliveryFrom, jwt: string): Promise<BaseResponse<Delivery>> => {
    console.log(model);

    const response = await axios
        .put<UpdateDeliveryFrom, AxiosResponse<BaseResponse<Delivery>>>(
            `${Config.serverUrl}${Endpoints.Delivery.Update}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Delivery>(error));

    return response;
};

export const UpdateDeliveryAction = (): MutationProcessing<UpdateDeliveryFrom, BaseResponse<Delivery>> => {
    const { userStore } = UseStore();

    const { mutateAsync, isLoading } = useMutation((model: UpdateDeliveryFrom) =>
        UpdateDeliveryRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const GetDeliveryByHashRequest = async (hash: string, jwt: string): Promise<BaseResponse<Delivery>> => {
    const response = await axios
        .get<null, AxiosResponse<BaseResponse<Delivery>>>(`${Config.serverUrl}${Endpoints.Delivery.GetByHash(hash)}`, {
            headers: GetHeader(jwt),
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Delivery>(error));

    return response;
};

export const GetDeliveryByHash = (hash: string): FetchProcessing<Delivery> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery(["fetch-delivery", hash], () =>
        GetDeliveryByHashRequest(hash, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};
