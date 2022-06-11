export const isNullOrWhiteSpace = (value: string | null | undefined) => {
    if (value === null || value === undefined) {
        return true;
    }

    const trimValue = value?.trim();

    if (trimValue === "") {
        return true;
    }

    return false;
};
