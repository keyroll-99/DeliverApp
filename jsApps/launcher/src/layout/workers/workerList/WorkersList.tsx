import { CircularProgress } from "@mui/material";
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { useNavigate } from "react-router-dom";
import { GetWorkers } from "service/companyService/WorkersServices";
import Path, { GetPathWithParam } from "utils/route/Path";

const columns: GridColDef[] = [
    { field: "username", headerName: "Username" },
    { field: "name", headerName: "Name", width: 100 },
    { field: "surname", headerName: "Surname", width: 150 },
    { field: "email", headerName: "E-mail", width: 250 },
    { field: "roles", headerName: "Roles", width: 250 },
    {
        field: "phoneNumber",
        headerName: "Phone number",
        width: 125,
    },
];

const baseName = "workers-list";

const WorkersList = () => {
    const { isLoading, data, isSuccess } = GetWorkers();
    const navigation = useNavigate();
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
                    onCellClick={(cell) => navigation(GetPathWithParam(Path.editWorker, cell.id as string))}
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

export default WorkersList;
