import axios, { AxiosError, AxiosRequestHeaders, AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { BaseResponse, FetchProcessing } from "service/_core/Models";
import { UseStore } from "stores/Store";
import Endpoints from "utils/axios/Endpoints";
import GetHeader from "utils/axios/GetHeader";
import Config from "utils/_core/Config";
import RoleResponse from "./models/RoleResponse";

const FetchRole = async (header: AxiosRequestHeaders): Promise<BaseResponse<RoleResponse[]>> => {
    const response = await axios
        .get<null, AxiosResponse<BaseResponse<RoleResponse[]>>>(`${Config.serverUrl}${Endpoints.Role.GetAll}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch(
            (error: AxiosError) =>
                ({
                    isSuccess: false,
                    error: error.message,
                } as BaseResponse<RoleResponse[]>)
        );

    return response;
};

export const GetRoles = (): FetchProcessing<RoleResponse[]> => {
    const { userStore } = UseStore();
    const header = GetHeader(userStore.getUser!.jwt);
    const { data, isLoading } = useQuery("fetch roles", () => FetchRole(header), { cacheTime: Infinity });

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};
