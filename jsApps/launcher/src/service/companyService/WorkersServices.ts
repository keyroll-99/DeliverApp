import axios, { AxiosError, AxiosRequestHeaders } from "axios";
import { useQuery } from "react-query";
import { UseStore } from "../../stores/Store";
import Endpoints from "../../utils/axios/Endpoints";
import GetHeader from "../../utils/axios/GetHeader";
import Config from "../../utils/_core/Config";
import UserResponse from "../userService/models/UserResponse";
import { BaseResponse, FetchProcessing } from "../_core/Models";

const FetchWorkers = async (header: AxiosRequestHeaders): Promise<BaseResponse<UserResponse[]>> => {
    const response = await axios
        .get<BaseResponse<UserResponse[]>>(`${Config.serverUrl}${Endpoints.Company.Workers}`, {
            headers: header,
        })
        .then((resp) => resp.data)
        .catch(
            (err: AxiosError) =>
                ({
                    isSuccess: false,
                    error: err.message,
                } as BaseResponse<UserResponse[]>)
        );
    return response;
};

export const GetWorkers = (): FetchProcessing<UserResponse[]> => {
    const { userStore } = UseStore();

    const header = GetHeader(userStore.getUser!.jwt);
    const { data, isLoading } = useQuery("fetch workers", () => FetchWorkers(header), { cacheTime: Infinity });

    return {
        isLoading: isLoading,
        data: data?.data,
        error: data?.error,
        isSuccess: data?.isSuccess,
    };
};
