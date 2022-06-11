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
    refresh?: () => void;
}

export interface MutationProcessing<TRequest, TResponse> extends FetchProcessing<TResponse> {
    mutateAsync: (data: TRequest) => Promise<TResponse>;
}
