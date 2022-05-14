import { render } from "@testing-library/react";
import { useParams } from "react-router-dom";
import { GetLocationByHashAction, UpdateLocationAction } from "service/location/LocationService";
import EditLocation, { urlParam } from "./EditLocation";

jest.mock("service/location/LocationService", () => ({
    UpdateLocationAction: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    ...jest.requireActual("react-router-dom"),
    useParams: jest.fn(),
}));

jest.mock("service/location/LocationService", () => ({
    UpdateLocationAction: jest.fn(),
    GetLocationByHashAction: jest.fn(),
}));

describe("CreateLocation", () => {
    beforeEach(() => {
        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({ locationHash: "hash" } as urlParam);
        (GetLocationByHashAction as jest.MockedFunction<typeof GetLocationByHashAction>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: {
                city: "c",
                country: "c",
                email: "e",
                hash: "s",
                no: "b",
                phoneNumber: "222",
                postalCode: "87-100",
                region: "region",
                street: "street",
            },
        });
    });

    test("should render form", () => {
        // arrange
        (UpdateLocationAction as jest.MockedFunction<typeof UpdateLocationAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<EditLocation />);

        // assert
        expect(component.queryByText("Country")).toBeInTheDocument();
        expect(component.queryByText("City")).toBeInTheDocument();
        expect(component.queryByText("Region")).toBeInTheDocument();
        expect(component.queryByText("Postal Code")).toBeInTheDocument();
        expect(component.queryByText("Street")).toBeInTheDocument();
        expect(component.queryByText("No")).toBeInTheDocument();
        expect(component.queryByText("Email")).toBeInTheDocument();
        expect(component.queryByText("Phone number")).toBeInTheDocument();
    });
});
