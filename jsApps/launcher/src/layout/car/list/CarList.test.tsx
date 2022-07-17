import { render } from "@testing-library/react";
import { GetCarsListAction } from "service/carService/CarService";
import CarsList from "./CarsList";

jest.mock("service/carService/CarService", () => ({
    GetCarsListAction: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn().mockReturnValue(jest.fn()),
}));

jest.mock("@mui/x-data-grid", () => ({
    DataGrid: () => <div></div>,
    GridColDef: () => <div></div>,
    GridToolbar: () => <div></div>,
}));

describe(CarsList, () => {
    test("should math to snapshot", () => {
        (GetCarsListAction as jest.MockedFunction<typeof GetCarsListAction>).mockReturnValue({
            isLoading: false,
            data: [{ brand: "test", model: "test", registrationNumber: "test", vin: "test", hash: "test" }],
            isSuccess: true,
        });

        const component = render(<CarsList />);

        expect(component).toMatchSnapshot();
    });
});
