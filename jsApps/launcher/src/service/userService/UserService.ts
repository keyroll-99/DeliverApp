import axios, { AxiosError, AxiosRequestHeaders, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import { BaseResponse, FetchProcessing, MutationProcessing } from "../_core/Models";
import ChangePasswordForm from "./models/ChangePasswordForm";
import CreateUserForm from "./models/CreateUserForm";
import UpdateUserForm from "./models/UpdateUserForm";
import UserResponse from "./models/UserResponse";

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
        .catch(
            (error: AxiosError) =>
                ({
                    isSuccess: false,
                    error: error.message,
                } as BaseResponse<UserResponse>)
        );
    return response;
};

export const CreateUser = (): MutationProcessing<CreateUserForm, BaseResponse<UserResponse>> => {
    const { userStore } = UseStore();
    const header = GetHeader(userStore.getUser!.jwt);

    const { isLoading, mutate, mutateAsync, data } = useMutation(
        (form: CreateUserForm) => CreateUserRequest(form, header),
        {
            onMutate: (form) => {
                form.companyHash = userStore.getUser!.companyHash;
            },
        }
    );

    return {
        isLoading: isLoading,
        mutate: mutate,
        mutateAsync: mutateAsync,
        data: data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};

const GetUserRequest = async (hash: string, jwt: string): Promise<BaseResponse<UserResponse>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .get<null, AxiosResponse<BaseResponse<UserResponse>>>(`${Config.serverUrl}${Endpoints.User.GetUser(hash)}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<UserResponse>));

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

const ChangePasswordRequest = async (request: ChangePasswordForm, jwt: string): Promise<BaseResponse<null>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .put<ChangePasswordForm, AxiosResponse<BaseResponse<null>>>(
            `${Config.serverUrl}${Endpoints.User.ChagnePassword}`,
            request,
            { headers: header }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<null>));

    return response;
};

export const ChangePasswordAction = (): MutationProcessing<ChangePasswordForm, BaseResponse<null>> => {
    const { userStore } = UseStore();

    const { isLoading, data, mutate, mutateAsync } = useMutation((form: ChangePasswordForm) =>
        ChangePasswordRequest(form, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutate: mutate,
        mutateAsync: mutateAsync,
        data: data,
        error: data?.error,
        isSuccess: data?.isSuccess,
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
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<UserResponse>));

    return response;
};

export const UpdateUserAction = (): MutationProcessing<UpdateUserForm, BaseResponse<UserResponse>> => {
    const { userStore } = UseStore();

    const { isLoading, data, mutate, mutateAsync } = useMutation("update user", (form: UpdateUserForm) =>
        UpdateUserRequest(form, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutate: mutate,
        mutateAsync: mutateAsync,
        data: data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};
