import axios from "axios";
import { useQuery } from "react-query";
import { FetchProcessing } from "service/_core/Models";

interface ConfigType {
    serverUrl: string;
}

const Config: ConfigType = {
    serverUrl: "",
};

const fetchConfig = async (): Promise<ConfigType> => {
    return await axios.get(`${process.env.PUBLIC_URL}/config.json`).then((resp) => resp.data);
};

export const LoadConfig = (): FetchProcessing<ConfigType> => {
    const { isLoading, data } = useQuery("fetch appsetings", fetchConfig, {
        onSuccess: (data) => {
            Object.assign(Config, data);
        },
        cacheTime: -1,
    });

    return {
        isLoading: isLoading,
        data: data,
    };
};

export default Config;
