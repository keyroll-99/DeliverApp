import { AxiosRequestHeaders } from "axios";

const GetHeader = (jwt: string): AxiosRequestHeaders => {
    return {
        Authorization: jwt,
    };
};

export default GetHeader;
