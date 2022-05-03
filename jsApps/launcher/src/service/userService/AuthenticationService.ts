import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { useNavigate } from "react-router-dom";
import Path from "utils/route/Path";
import { UseStore } from "../../stores/Store";
import Endpoints from "../../utils/axios/Endpoints";
import Config from "../../utils/_core/Config";
import { BaseResponse, FetchProcessing, MutationProcessing } from "../_core/Models";
import AuthResponse from "./models/AuthResponse";
import LoginForm from "./models/LoginForm";

export const LoginRequest = async (form: LoginForm): Promise<BaseResponse<AuthResponse>> => {
    const response = await axios
        .post<LoginForm, AxiosResponse<BaseResponse<AuthResponse>>>(
            `${Config.serverUrl}${Endpoints.Authentication.Login}`,
            form,
            {
                withCredentials: true,
            }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => {
            return {
                isSuccess: false,
                error: error.message,
            } as BaseResponse<AuthResponse>;
        });
    return response;
};

const RefreshTokenRequest = async (): Promise<BaseResponse<AuthResponse>> => {
    const response = await axios
        .post<undefined, AxiosResponse<BaseResponse<AuthResponse>>>(
            `${Config.serverUrl}${Endpoints.Authentication.Refresh}`,
            null,
            {
                withCredentials: true,
            }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => {
            return {
                isSuccess: false,
                error: error.message,
            } as BaseResponse<AuthResponse>;
        });
    return response;
};

export const RefreshToken = (): FetchProcessing<AuthResponse> => {
    const { userStore } = UseStore();
    const { isLoading, data } = useQuery("RefreshToken", RefreshTokenRequest, { refetchInterval: 180000 });

    if (!isLoading && data?.isSuccess) {
        userStore.setUser(data!.data!);
    }

    return {
        isLoading: isLoading,
        error: data?.error,
        isSuccess: data?.isSuccess,
        data: data?.data,
    };
};

export const Login = (): MutationProcessing<LoginForm, BaseResponse<AuthResponse>> => {
    const { userStore } = UseStore();
    const navigation = useNavigate();

    const { isLoading, data, mutate, mutateAsync } = useMutation((request: LoginForm) => LoginRequest(request), {
        onSuccess: (result) => {
            if (result?.isSuccess) {
                console.log("successLogin", result);

                userStore.setUser(result!.data!);
            } else {
                console.log("go back", result);

                navigation(Path.login);
            }
        },
    });

    return {
        isLoading: isLoading,
        error: data?.error,
        isSuccess: data?.isSuccess,
        mutate: mutate,
        data: data,
        mutateAsync: mutateAsync,
    };
};
