import { Button, FormControl, FormHelperText, Grid } from "@mui/material";
import { useState } from "react";
import CreateClass from "utils/style/CreateClass";
import TransferList from "./TransferList";
import type { transferListItemType } from "./TransferListType";

interface props {
    selectedItems: transferListItemType[];
    availableItems: transferListItemType[];
    leftListTitle: string;
    rightListTitle: string;
    baseClass: string;
    error?: string;
    toSelected: (index: number[]) => void;
    toAvaliable: (index: number[]) => void;
}

const TransferListGrid = ({
    selectedItems,
    availableItems,
    toAvaliable,
    toSelected,
    error,
    baseClass,
    leftListTitle,
    rightListTitle,
}: props) => {
    const [checkedSelectedItem, setCheckedSelectedItem] = useState<number[]>([]);
    const [checkedAvailableItem, setCheckedAvailableItem] = useState<number[]>([]);

    const toggleSelectSelected = (indexs: number[]) => {
        const shouldUncheck = checkedSelectedItem.some((x) => indexs.some((y) => y === x));

        if (shouldUncheck) {
            setCheckedSelectedItem(checkedSelectedItem.filter((x) => indexs.every((y) => y !== x)));
        } else {
            setCheckedSelectedItem([...checkedSelectedItem, ...indexs]);
        }
    };

    const toggleSelectAvailable = (indexs: number[]) => {
        const shouldUncheck = checkedAvailableItem.some((x) => indexs.some((y) => y === x));

        if (shouldUncheck) {
            setCheckedAvailableItem(checkedAvailableItem.filter((x) => indexs.every((y) => y !== x)));
        } else {
            setCheckedAvailableItem([...checkedAvailableItem, ...indexs]);
        }
    };

    const onClickToAvaliable = () => {
        setCheckedAvailableItem([]);
        setCheckedSelectedItem([]);
        toAvaliable(checkedSelectedItem);
    };

    const onClickToSeleted = () => {
        setCheckedAvailableItem([]);
        setCheckedSelectedItem([]);
        toSelected(checkedAvailableItem);
    };

    return (
        <FormControl error={error ? true : false}>
            <Grid
                container
                spacing={2}
                justifyContent="center"
                alignItems="center"
                className={CreateClass(baseClass, "grid")}
            >
                <Grid item className={CreateClass(baseClass, "grid-list")}>
                    <TransferList
                        checkeds={checkedAvailableItem}
                        elements={availableItems}
                        toggleChoose={toggleSelectAvailable}
                        title={leftListTitle}
                    />
                </Grid>
                <Grid item>
                    <Grid container direction="column" alignItems="center">
                        <Button role={"to-seleted"} variant="outlined" size="small" onClick={onClickToSeleted}>
                            &gt;
                        </Button>
                        <Button role={"to-avaliable"} variant="outlined" size="small" onClick={onClickToAvaliable}>
                            &lt;
                        </Button>
                    </Grid>
                </Grid>
                <Grid item className={CreateClass(baseClass, "grid-list")}>
                    <TransferList
                        checkeds={checkedSelectedItem}
                        elements={selectedItems}
                        toggleChoose={toggleSelectSelected}
                        title={rightListTitle}
                    />
                </Grid>
            </Grid>
            <FormHelperText>{error}</FormHelperText>
        </FormControl>
    );
};

export default TransferListGrid;
