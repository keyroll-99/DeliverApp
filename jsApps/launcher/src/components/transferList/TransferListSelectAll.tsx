import { Checkbox } from "@mui/material";

interface props {
    isChecked: boolean;
    isIndeterminate: boolean;
    isDisabled: boolean;
    onClick: () => void;
}

const TransferListSelectAll = ({ isChecked, isIndeterminate, isDisabled, onClick }: props) => {
    return <Checkbox onClick={onClick} checked={isChecked} indeterminate={isIndeterminate} disabled={isDisabled} />;
};

export default TransferListSelectAll;
