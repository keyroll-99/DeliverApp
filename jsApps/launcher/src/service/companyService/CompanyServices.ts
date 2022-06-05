import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery } from "react-query";
import { BaseResponse, FetchProcessing, MutationProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import AssignUserToCompanyForm from "./models/AssignUserToCompanyForm";
import Company from "./models/Company";
import CreateCompanyForm from "./models/CreateCompanyFormt";

const CreateCompanyRequest = async (model: CreateCompanyForm, jwt: string): Promise<BaseResponse<Company>> => {
    const response = await axios
        .post<CreateCompanyForm, AxiosResponse<BaseResponse<Company>>>(
            `${Config.serverUrl}${Endpoints.Company.Create}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<Company>));

    return response;
};

export const CreateCompanyAction = (): MutationProcessing<CreateCompanyForm, BaseResponse<Company>> => {
    const { userStore } = UseStore();

    const { mutateAsync, isLoading } = useMutation("create-company", (model: CreateCompanyForm) =>
        CreateCompanyRequest(model, userStore.getUser!.jwt)
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};

const GetCompaniesRequest = async (jwt: string): Promise<BaseResponse<Company[]>> => {
    const response = (await axios.get)<BaseResponse<Company[]>>(`${Config.serverUrl}${Endpoints.Company.List}`, {
        headers: GetHeader(jwt),
    })
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<Company[]>));

    return response;
};

export const GetCompaniesAction = (): FetchProcessing<Company[]> => {
    const { userStore } = UseStore();

    const { isLoading, data } = useQuery("GetCompanyAction", () => GetCompaniesRequest(userStore.getUser?.jwt ?? ""));

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};

const AssignUserToCompanyRequest = async (
    model: AssignUserToCompanyForm,
    jwt: string
): Promise<BaseResponse<undefined>> => {
    const response = axios
        .put<AssignUserToCompanyForm, AxiosResponse<BaseResponse<undefined>>>(
            `${Config.serverUrl}${Endpoints.Company.AssingUserToCompany}`,
            model,
            { headers: GetHeader(jwt) }
        )
        .then((resp) => resp.data)
        .catch((error: AxiosError) => ({ isSuccess: false, error: error.message } as BaseResponse<undefined>));

    return response;
};

export const AssignUserToCompanyAction = (): MutationProcessing<AssignUserToCompanyForm, BaseResponse<undefined>> => {
    const { userStore } = UseStore();

    const { isLoading, mutateAsync } = useMutation("AssingUserToCompanyAction", (model: AssignUserToCompanyForm) =>
        AssignUserToCompanyRequest(model, userStore.getUser?.jwt ?? "")
    );

    return {
        isLoading: isLoading,
        mutateAsync: mutateAsync,
    };
};
