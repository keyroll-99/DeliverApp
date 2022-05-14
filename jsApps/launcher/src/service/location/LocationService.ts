import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { BaseResponse, FetchProcessing, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import CreateLocationForm from "./models/CreateLocationForm";
import Location from "./models/Location";
import UpdateLocationForm from "./models/UpdateLocationForm";

const CreateLocationRequest = async (model: CreateLocationForm, jwt: string): Promise<BaseResponse<Location>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .post<CreateLocationForm, AxiosResponse<BaseResponse<Location>>>(
            `${Config.serverUrl}${Endpoints.Location.Create}`,
            model,
            {
                headers: header,
            }
        )
        .then((resp) => resp.data)
        .catch(
            (error: AxiosError) =>
                ({
                    isSuccess: false,
                    error: error.message,
                } as BaseResponse<Location>)
        );

    return response;
};

export const CreateLocationAction = (): MutationProcessing<CreateLocationForm, BaseResponse<Location>> => {
    const { userStore } = UseStore();
    const { isLoading, mutateAsync } = useMutation("create-location", (model: CreateLocationForm) =>
        CreateLocationRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const GetLocationListRequest = async (jwt: string): Promise<BaseResponse<Location[]>> => {
    const header = GetHeader(jwt);

    const resposne = await axios
        .get<null, AxiosResponse<BaseResponse<Location[]>>>(`${Config.serverUrl}${Endpoints.Location.GetList}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<Location[]>));

    return resposne;
};

export const GetLocationListAcion = (): FetchProcessing<Location[]> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery("Get-Locations", () => GetLocationListRequest(userStore.getUser!.jwt));

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};

const UpdateLocationRequest = async (model: UpdateLocationForm, jwt: string): Promise<BaseResponse<Location>> => {
    const response = await axios
        .put<UpdateLocationForm, AxiosResponse<BaseResponse<Location>>>(
            `${Config.serverUrl}${Endpoints.Location.Update}`,
            model,
            {
                headers: GetHeader(jwt),
            }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<Location>));

    return response;
};

export const UpdateLocationAction = (): MutationProcessing<UpdateLocationForm, BaseResponse<Location>> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation("update-location", (model: UpdateLocationForm) =>
        UpdateLocationRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const GetLocationByHashRequest = async (hash: string, jwt: string): Promise<BaseResponse<Location>> => {
    const response = await axios
        .get<null, AxiosResponse<BaseResponse<Location>>>(`${Config.serverUrl}${Endpoints.Location.GetByHash(hash)}`, {
            headers: GetHeader(jwt),
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<Location>));

    return response;
};

export const GetLocationByHashAction = (hash: string): FetchProcessing<Location> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery(
        ["get-location-by-hash", hash],
        () => GetLocationByHashRequest(hash, userStore.getUser!.jwt),
        {
            cacheTime: Infinity,
            refetchInterval: false,
            refetchIntervalInBackground: false,
        }
    );

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};
