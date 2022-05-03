import { Snackbar as Snack } from "@mui/material";
import MuiAlert from "@mui/material/Alert";
import React from "react";

interface props {
    message: string;
    isOpen?: boolean;
    variant: "success" | "error";
    setIsOpen: (value: boolean) => void;
}

const Snackbar = ({ message, isOpen, setIsOpen, variant }: props) => {
    const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === "clickaway") {
            return;
        }

        setIsOpen(false);
    };

    return (
        <Snack open={isOpen} autoHideDuration={6000} onClose={handleClose}>
            <MuiAlert onClose={handleClose} color={variant} sx={{ width: "100%" }} variant="filled" elevation={6}>
                {message}
            </MuiAlert>
        </Snack>
    );
};

export default Snackbar;
