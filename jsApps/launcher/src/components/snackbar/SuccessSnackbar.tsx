import { Snackbar } from "@mui/material";
import { useState } from "react";
import MuiAlert from "@mui/material/Alert";
import React from "react";

interface props {
    message: string;
    isOpen?: boolean;
}

const SuccessSnackbar = ({ message, isOpen }: props) => {
    const [open, setOpen] = useState(isOpen);

    const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === "clickaway") {
            return;
        }

        setOpen(false);
    };

    return (
        <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
            <MuiAlert onClose={handleClose} security="success" sx={{ width: "100%" }} variant="filled" elevation={6}>
                {message}
            </MuiAlert>
        </Snackbar>
    );
};

export default SuccessSnackbar;
