import { CircularProgress } from "@mui/material";
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { useNavigate } from "react-router-dom";
import { GetCarsListAction } from "service/carService/CarService";

const columns: GridColDef[] = [
    {
        field: "brand",
        headerName: "Brand",
        width: 200,
        renderCell: (param) => <p>{(param.value as string)?.toUpperCase()}</p>,
    },
    {
        field: "model",
        headerName: "Model",
        width: 200,
        renderCell: (param) => <p>{(param.value as string)?.toUpperCase()}</p>,
    },
    {
        field: "registrationNumber",
        headerName: "Registration number",
        width: 200,
        renderCell: (param) => <p>{(param.value as string)?.toUpperCase()}</p>,
    },
    {
        field: "vin",
        headerName: "VIN",
        width: 200,
        renderCell: (param) => <p>{(param.value as string)?.toUpperCase()}</p>,
    },
];

const baseClass = "car-list";

const CarsList = () => {
    const navigation = useNavigate();
    const { isLoading, isSuccess, data } = GetCarsListAction();

    if (!isSuccess && !isLoading) {
        return <h1>Something went wrong, please refresh page</h1>;
    }

    return (
        <div className={baseClass}>
            {data && !isLoading ? (
                <DataGrid
                    getRowId={(row) => row.hash}
                    columns={columns}
                    rows={data!}
                    rowsPerPageOptions={[20, 100]}
                    loading={isLoading}
                    components={{
                        Toolbar: GridToolbar,
                    }}
                    disableDensitySelector={true}
                />
            ) : (
                <CircularProgress />
            )}
        </div>
    );
};

export default CarsList;
