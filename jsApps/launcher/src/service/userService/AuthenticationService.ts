import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { useNavigate } from "react-router-dom";
import { HandleApiError } from "service/_core/HandleApiError";
import GetHeader from "utils/axios/GetHeader";
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
        .catch((error: AxiosError) => HandleApiError<AuthResponse>(error));
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
        .catch((error: AxiosError) => HandleApiError<AuthResponse>(error));
    return response;
};

export const RefreshToken = (): FetchProcessing<AuthResponse> => {
    const { userStore } = UseStore();
    const { isLoading, data } = useQuery("RefreshToken", RefreshTokenRequest, {
        refetchInterval: 180000,
        onError: () => {
            userStore.logout();
        },
        onSuccess: (response) => {
            if (!response.isSuccess) {
                userStore.logout();
            }
        },
    });

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

    const { isLoading, data, mutateAsync } = useMutation((request: LoginForm) => LoginRequest(request), {
        onSuccess: (result) => {
            if (result?.isSuccess) {
                userStore.setUser(result!.data!);
            } else {
                navigation(Path.login);
            }
        },
    });

    return {
        isLoading: isLoading,
        error: data?.error,
        isSuccess: data?.isSuccess,
        data: data,
        mutateAsync: mutateAsync,
    };
};

const LogoutRequest = async (jwt: string): Promise<BaseResponse<null>> => {
    const reponse = await axios
        .post<null, AxiosResponse<BaseResponse<null>>>(
            `${Config.serverUrl}${Endpoints.Authentication.Logout}`,
            {},
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return reponse;
};

export const Logout = (): MutationProcessing<undefined, BaseResponse<null>> => {
    const { userStore } = UseStore();

    const { isLoading, data, mutateAsync } = useMutation("logout", () => LogoutRequest(userStore.getUser!.jwt));

    return {
        isLoading: isLoading,
        data: data,
        error: data?.error,
        isSuccess: data?.isSuccess,
        mutateAsync: mutateAsync,
    };
};
