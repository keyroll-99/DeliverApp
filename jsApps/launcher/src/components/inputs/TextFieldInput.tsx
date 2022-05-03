import { Box, TextField } from "@mui/material";
import { ReactNode } from "react";
import CreateClass from "utils/style/CreateClass";

interface props {
    baseClass: string;
    label: string;
    error?: string | null;
    icon?: ReactNode;
    value?: string;
    type: "text" | "password" | "email" | "tel";
    onChange: (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void;
}

const TextFieldInput = ({ baseClass, label, onChange, error, icon, value, type }: props) => {
    return (
        <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
            {icon}
            <TextField
                label={label}
                variant="standard"
                onChange={onChange}
                value={value}
                error={error ? true : false}
                helperText={error}
                type={type}
            />
        </Box>
    );
};

export default TextFieldInput;
