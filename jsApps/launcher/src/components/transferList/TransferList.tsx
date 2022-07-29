import { Card, CardHeader, Checkbox, Divider, List, ListItem, ListItemIcon, ListItemText } from "@mui/material";
import TransferListSelectAll from "./TransferListSelectAll";
import type { transferListItemType } from "./TransferListType";

interface props {
    title: string;
    elements: transferListItemType[];
    checkeds: number[];
    toggleChoose: (index: number[]) => void;
}

const TransferList = ({ elements, toggleChoose, checkeds, title }: props) => {
    const isAllChecked = checkeds.length === elements.length && elements.length > 0;
    const isAnyChecked = checkeds.length > 0;

    const toggleAll = () => {
        if (isAllChecked) {
            toggleChoose(elements.map((x) => x.key));
        } else {
            toggleChoose(elements.filter((x) => checkeds.every((y) => y !== x.key)).map((x) => x.key));
        }
    };

    return (
        <Card>
            <CardHeader
                title={title}
                avatar={
                    <TransferListSelectAll
                        isChecked={isAllChecked}
                        isDisabled={elements.length <= 0}
                        isIndeterminate={!isAllChecked && isAnyChecked}
                        onClick={toggleAll}
                    />
                }
            />
            <Divider />
            <List role="list" component="div">
                {elements.map((element) => (
                    <ListItem key={element.key} role="listitem" button onClick={() => toggleChoose([element.key])}>
                        <ListItemIcon>
                            <Checkbox checked={checkeds.some((x) => x === element.key)} tabIndex={-1} disableRipple />
                        </ListItemIcon>
                        <ListItemText primary={element.value} />
                    </ListItem>
                ))}
            </List>
        </Card>
    );
};

export default TransferList;
