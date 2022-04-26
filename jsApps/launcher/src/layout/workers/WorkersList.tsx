import { CircularProgress } from "@mui/material";
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { GetWorkers } from "service/companyService/WorkersServices";

const columns: GridColDef[] = [
    { field: "username", headerName: "Username" },
    { field: "name", headerName: "Name", width: 100 },
    { field: "surname", headerName: "Surname", width: 150 },
    { field: "email", headerName: "E-mail", width: 250 },
    { field: "roles", headerName: "Roles", width: 250 },
];

const baseName = "workersList";

const WorkersList = () => {
    const { isLoading, data, isSuccess } = GetWorkers();
    if (!isSuccess && !isLoading) {
        return <>something went wrong</>;
    }

    return (
        <div className={baseName}>
            {data && !isLoading ? (
                <DataGrid
                    getRowId={(row) => row.hash}
                    columns={columns}
                    rows={data!}
                    rowsPerPageOptions={[20, 100]}
                    loading={isLoading}
                    // TODO: redirect to update
                    onCellClick={(cell) => console.log(cell.id)}
                    components={{
                        Toolbar: GridToolbar,
                    }}
                />
            ) : (
                <CircularProgress />
            )}
        </div>
    );
};

export default WorkersList;
