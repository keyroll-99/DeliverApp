import axios, { AxiosError, AxiosRequestHeaders, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { HandleApiError } from "service/_core/HandleApiError";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import { BaseResponse, FetchProcessing, MutationProcessing } from "../_core/Models";
import CreateUserForm from "./models/UserModels/CreateUserForm";
import UpdateUserForm from "./models/UserModels/UpdateUserForm";
import UserResponse from "./models/UserModels/UserResponse";

const CreateUserRequest = async (
    form: CreateUserForm,
    header: AxiosRequestHeaders
): Promise<BaseResponse<UserResponse>> => {
    const response = await axios
        .post<CreateUserForm, AxiosResponse<BaseResponse<UserResponse>>>(
            `${Config.serverUrl}${Endpoints.User.Create}`,
            form,
            { headers: header }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<UserResponse>(error));
    return response;
};

export const CreateUser = (): MutationProcessing<CreateUserForm, BaseResponse<UserResponse>> => {
    const { userStore } = UseStore();
    const header = GetHeader(userStore.getUser!.jwt);

    const { isLoading, mutateAsync } = useMutation((form: CreateUserForm) => CreateUserRequest(form, header), {
        onMutate: (form) => {
            form.companyHash = userStore.getUser!.companyHash;
        },
    });

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const GetUserRequest = async (hash: string, jwt: string): Promise<BaseResponse<UserResponse>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .get<null, AxiosResponse<BaseResponse<UserResponse>>>(`${Config.serverUrl}${Endpoints.User.GetUser(hash)}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<UserResponse>(error));

    return response;
};

export const GetUser = (
    hash: string,
    onSuccess?: (result: BaseResponse<UserResponse>) => void
): FetchProcessing<UserResponse> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery(["GetUser", hash], () => GetUserRequest(hash, userStore.getUser!.jwt), {
        cacheTime: Infinity,
        onSuccess: onSuccess,
    });

    return {
        isLoading: isLoading,
        isSuccess: data?.isSuccess,
        data: data?.data,
        error: data?.error,
    };
};

const UpdateUserRequest = async (request: UpdateUserForm, jwt: string): Promise<BaseResponse<UserResponse>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .put<UpdateUserForm, AxiosResponse<BaseResponse<UserResponse>>>(
            `${Config.serverUrl}${Endpoints.User.UpdateUser}`,
            request,
            {
                headers: header,
            }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<UserResponse>(error));

    return response;
};

export const UpdateUserAction = (): MutationProcessing<UpdateUserForm, BaseResponse<UserResponse>> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation("update user", (form: UpdateUserForm) =>
        UpdateUserRequest(form, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const FetchWorkers = async (header: AxiosRequestHeaders): Promise<BaseResponse<UserResponse[]>> => {
    const response = await axios
        .get<BaseResponse<UserResponse[]>>(`${Config.serverUrl}${Endpoints.User.UserList}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<UserResponse[]>(error));
    return response;
};

export const GetWorkers = (): FetchProcessing<UserResponse[]> => {
    const { userStore } = UseStore();

    const header = GetHeader(userStore.getUser!.jwt);
    const { data, isLoading, refetch } = useQuery("fetch workers", () => FetchWorkers(header), { cacheTime: Infinity });

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
        refresh: refetch,
    };
};

const FireUserRequest = async (userHash: string, jwt: string): Promise<BaseResponse<null>> => {
    const response = await axios
        .put<null, AxiosResponse<BaseResponse<null>>>(
            `${Config.serverUrl}${Endpoints.User.Fire(userHash)}`,
            undefined,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const FireUserAction = (): MutationProcessing<string, BaseResponse<null>> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((hash: string) =>
        FireUserRequest(hash, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};
