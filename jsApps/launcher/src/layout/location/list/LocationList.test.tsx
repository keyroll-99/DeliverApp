import { render } from "@testing-library/react";
import { GetLocationListAcion } from "service/location/LocationService";
import LocationList from "./LocationList";

jest.mock("service/location/LocationService", () => ({
    GetLocationListAcion: jest.fn(),
}));

jest.mock("@mui/x-data-grid", () => ({
    DataGrid: () => <div>DataGrid</div>,
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn().mockReturnValue(jest.fn()),
}));

describe("LocationList", () => {
    test("should render error when api return error", () => {
        // arrange
        (GetLocationListAcion as jest.MockedFunction<typeof GetLocationListAcion>).mockReturnValue({
            isLoading: false,
            isSuccess: false,
        });

        // act
        const component = render(<LocationList />);

        // assert
        expect(component.queryByText("something went wrong")).toBeInTheDocument();
    });

    test("should render loader when isloading is true", () => {
        // arrange
        (GetLocationListAcion as jest.MockedFunction<typeof GetLocationListAcion>).mockReturnValue({
            isLoading: true,
            isSuccess: false,
        });

        // act
        const component = render(<LocationList />);

        // assert
        expect(component.queryByText("something went wrong")).not.toBeInTheDocument();
        expect(component.container.querySelector(".MuiCircularProgress-circle")).toBeInTheDocument();
    });

    test("should render grid after fetch data", () => {
        // arrange
        (GetLocationListAcion as jest.MockedFunction<typeof GetLocationListAcion>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: [
                {
                    city: "city",
                    country: "country",
                    email: "email",
                    hash: "hash",
                    no: "no",
                    phoneNumber: "phoneNumber",
                    postalCode: "postalCode",
                    region: "region",
                    street: "street",
                },
            ],
        });

        // act
        const component = render(<LocationList />);

        // assert
        expect(component.queryByText("DataGrid")).toBeInTheDocument();
    });
});
