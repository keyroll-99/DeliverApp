import axios, { AxiosError, AxiosRequestHeaders, AxiosResponse } from "axios";
import { useMutation } from "react-query";
import { UseStore } from "../../stores/Store";
import Endpoints from "../../utils/axios/Endpoints";
import GetHeader from "../../utils/axios/GetHeader";
import Config from "../../utils/_core/Config";
import { BaseResponse, MutationProcessing } from "../_core/Models";
import CreateUserForm from "./models/CreateUserForm";
import UserResponse from "./models/UserResponse";

export const CreateUserRequest = async (
    form: CreateUserForm,
    header: AxiosRequestHeaders
): Promise<BaseResponse<UserResponse>> => {
    const response = await axios
        .post<CreateUserForm, AxiosResponse<BaseResponse<UserResponse>>>(
            `${Config.serverUrl}${Endpoints.User.Create}`,
            form,
            header
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

    const { isLoading, mutate, mutateAsync, data } = useMutation((form: CreateUserForm) =>
        CreateUserRequest(form, header)
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
