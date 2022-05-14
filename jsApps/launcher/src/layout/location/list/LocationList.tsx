import { CircularProgress } from "@mui/material";
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { useNavigate } from "react-router-dom";
import { GetLocationListAcion } from "service/location/LocationService";
import Path, { GetPathWithParam } from "utils/route/Path";

const columns: GridColDef[] = [
    { field: "country", headerName: "Country" },
    { field: "city", headerName: "City" },
    {
        field: "region",
        headerName: "Region",
        width: 250,
    },
    { field: "postalCode", headerName: "Postal code" },
    { field: "street", headerName: "Street", width: 150 },
    { field: "no", headerName: "No" },
    { field: "email", headerName: "Email", width: 250 },
    { field: "phoneNumber", headerName: "Phone number", width: 125 },
];

const baseClass = "location-list";

const LocationList = () => {
    const navigation = useNavigate();
    const { isLoading, isSuccess, data } = GetLocationListAcion();

    if (!isSuccess && !isLoading) {
        return <h1>something went wrong</h1>;
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
                    onCellClick={(cell) => navigation(GetPathWithParam(Path.locationUpdate, cell.id as string))}
                />
            ) : (
                <CircularProgress />
            )}
        </div>
    );
};

export default LocationList;
