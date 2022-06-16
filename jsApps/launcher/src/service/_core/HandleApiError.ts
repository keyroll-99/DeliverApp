import { AxiosError } from "axios";
import { BaseResponse } from "./Models";

export const HandleApiError = <T = null>(error: AxiosError): BaseResponse<T> =>
    ({ ...error.response?.data } as BaseResponse<T>);
