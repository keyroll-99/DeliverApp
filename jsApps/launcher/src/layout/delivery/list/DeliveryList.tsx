import { CircularProgress, Tooltip } from "@mui/material";
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { useNavigate } from "react-router-dom";
import { GetDeliveryList } from "service/deliveryService/DeliveryService";
import { stringDeliveryStatus } from "service/deliveryService/models/DeliverStatus";
import Location from "service/location/models/Location";
import { LocationFullText, LocationShortText } from "utils/location/LocationUtils";
import Path, { GetPathWithParam } from "utils/route/Path";

const columns: GridColDef[] = [
    { width: 250, field: "name", headerName: "Name" },
    { width: 200, field: "startDate", headerName: "Pickup date" },
    { width: 200, field: "endDate", headerName: "Deliver date" },
    {
        width: 400,
        field: "from",
        headerName: "From",
        renderCell: (params) => (
            <Tooltip title={LocationFullText(params.value as Location)}>
                <p>{LocationShortText(params.value as Location)}</p>
            </Tooltip>
        ),
    },
    {
        width: 400,
        field: "to",
        headerName: "To",
        renderCell: (params) => (
            <Tooltip title={LocationFullText(params.value as Location)}>
                <p>{LocationShortText(params.value as Location)}</p>
            </Tooltip>
        ),
    },
    {
        field: "status",
        headerName: "Status",
        renderCell: (params) => <p>{stringDeliveryStatus(params.value as number)}</p>,
    },
];

const baseClassName = "delivery-list";

const DeliveryList = () => {
    const { isLoading, data } = GetDeliveryList();
    const navigation = useNavigate();

    if (isLoading) {
        return <CircularProgress />;
    }

    return (
        <div className={baseClassName}>
            <DataGrid
                getRowId={(row) => row.hash}
                columns={columns}
                disableDensitySelector={true}
                rows={data!}
                onRowClick={(param) => navigation(GetPathWithParam(Path.deliveryUpdate, param.id as string))}
                components={{
                    Toolbar: GridToolbar,
                }}
            />
        </div>
    );
};

export default DeliveryList;
