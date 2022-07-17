export interface BaseResponse<T = null> {
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

export interface MutationProcessing<TRequest, TResponse> {
    isLoading: boolean;
    mutateAsync: (data: TRequest) => Promise<TResponse>;
}
