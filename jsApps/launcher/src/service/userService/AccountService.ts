import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { HandleApiError } from "service/_core/HandleApiError";
import { BaseResponse, FetchProcessing, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import ChangePasswordForm from "./models/AccountModels/ChangePasswordForm";

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

    const { isLoading, data, mutateAsync } = useMutation((form: ChangePasswordForm) =>
        ChangePasswordRequest(form, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
        data: data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};

const IsValidRecoveryKeyRequest = async (key: string): Promise<BaseResponse<Boolean>> => {
    const reponse = await axios
        .get<null, AxiosResponse<BaseResponse<Boolean>>>(
            `${Config.serverUrl}${Endpoints.Account.PasswordRecoverySetNewPassword}`
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
