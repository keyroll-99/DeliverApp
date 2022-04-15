interface ConfigType {
    serverUrl: string;
}

const Config: ConfigType = {
    serverUrl: "",
};

export const LoadConfig = (loadedConfig: ConfigType) => {
    Object.assign(Config, loadedConfig);
};

export default Config;
