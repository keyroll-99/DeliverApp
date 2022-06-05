import { Box, FormControl, InputLabel, MenuItem, Select as MuiSelect, SelectChangeEvent } from "@mui/material";
import KeyValuePair from "service/_core/KeyValuePair";
import CreateClass from "utils/style/CreateClass";

interface props {
    values: KeyValuePair<string, string>[];
    value: string;
    baseClass: string;
    label: string;

    setValue: (e: SelectChangeEvent) => void;
}

const Select = ({ baseClass, label, values, value, setValue }: props) => {
    return (
        <Box className={CreateClass(baseClass, "box")} sx={{ display: "flex", alignItems: "flex-end" }}>
            <FormControl className={CreateClass(baseClass, "select")}>
                <InputLabel id={`${label}-${baseClass}`}>{label}</InputLabel>
                <MuiSelect label={label} labelId={`${label}-${baseClass}`} value={value} onChange={setValue}>
                    {values.map((item) => (
                        <MenuItem value={item.key} key={`${item.key}-${item.value}`}>
                            {item.value}
                        </MenuItem>
                    ))}
                </MuiSelect>
            </FormControl>
        </Box>
    );
};

export default Select;
