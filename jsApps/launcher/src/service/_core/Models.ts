import { UseMutateFunction } from "react-query";

export interface BaseResponse<T> {
    isSuccess: boolean;
    data?: T;
    error: string;
}

export interface FetchProcessing<TResponse> {
    isLoading: boolean;
    error?: string;
    isSuccess?: boolean;
    data?: TResponse;
}

export interface MutationProcessing<TRequest, TResponse> extends FetchProcessing<TResponse> {
    mutate: (data: TRequest) => any;
}
