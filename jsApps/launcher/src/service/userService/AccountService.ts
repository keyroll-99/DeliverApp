import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { HandleApiError } from "service/_core/HandleApiError";
import { BaseResponse, FetchProcessing, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import ChangePasswordForm from "./models/AccountModels/ChangePasswordForm";
import InitPasswordRecoveryForm from "./models/AccountModels/InitPasswordRecoveryForm";
import PasswordRecoveryForm from "./models/AccountModels/PasswordRecoveryForm";

const ChangePasswordRequest = async (request: ChangePasswordForm, jwt: string): Promise<BaseResponse<null>> => {
    const header = GetHeader(jwt);

    const response = await axios
        .put<ChangePasswordForm, AxiosResponse<BaseResponse<null>>>(
            `${Config.serverUrl}${Endpoints.Account.ChagnePassword}`,
            request,
            { headers: header }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError(error));

    return response;
};

export const ChangePasswordAction = (): MutationProcessing<ChangePasswordForm, BaseResponse<null>> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation((form: ChangePasswordForm) =>
        ChangePasswordRequest(form, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const IsValidRecoveryKeyRequest = async (key: string): Promise<BaseResponse<Boolean>> => {
    console.log(key);
    const reponse = await axios
        .get<null, AxiosResponse<BaseResponse<Boolean>>>(
            `${Config.serverUrl}${Endpoints.Account.PasswordRecoveryIsValidRecoveryKey(key)}`
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<Boolean>(error));

    return reponse;
};

export const IsValidRecoveryKeyAction = (recoveryKey: string): FetchProcessing<Boolean> => {
    const { isLoading, data, refetch } = useQuery("IsValidRecoveryKeyRequest", () =>
        IsValidRecoveryKeyRequest(recoveryKey)
    );
    return {
        isLoading: isLoading,
        data: data?.data,
        isSuccess: data?.isSuccess,
        error: data?.error,
        refresh: refetch,
    };
};

const InitPasswordRecovery = async (form: InitPasswordRecoveryForm): Promise<BaseResponse<boolean>> => {
    const response = await axios
        .post<InitPasswordRecoveryForm, AxiosResponse<BaseResponse<boolean>>>(
            `${Config.serverUrl}${Endpoints.Account.PasswordRecoveryInit}`,
            form
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<boolean>(error));

    return response;
};

export const InitPasswordRecoveryAction = (): MutationProcessing<InitPasswordRecoveryForm, BaseResponse<boolean>> => {
    const { isLoading, mutateAsync } = useMutation((form: InitPasswordRecoveryForm) => InitPasswordRecovery(form));

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const PasswordRecoveryRequest = async (form: PasswordRecoveryForm): Promise<BaseResponse<boolean>> => {
    const response = await axios
        .post<PasswordRecoveryForm, AxiosResponse<BaseResponse<boolean>>>(
            `${Config.serverUrl}${Endpoints.Account.PasswordRecoverySetNewPassword}`,
            form
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => HandleApiError<boolean>(error));

    return response;
};

export const PasswordRecoveryAction = (): MutationProcessing<PasswordRecoveryForm, BaseResponse<boolean>> => {
    const { isLoading, mutateAsync } = useMutation((form: PasswordRecoveryForm) => PasswordRecoveryRequest(form));

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};
