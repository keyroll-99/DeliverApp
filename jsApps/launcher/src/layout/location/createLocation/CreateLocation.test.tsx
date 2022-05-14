import { render } from "@testing-library/react";
import { CreateLocationAction } from "service/location/LocationService";
import CreateLocation from "./CreateLocation";

jest.mock("service/location/LocationService", () => ({
    CreateLocationAction: jest.fn(),
}));

describe("CreateLocation", () => {
    test("should render form", () => {
        // arrange
        (CreateLocationAction as jest.MockedFunction<typeof CreateLocationAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<CreateLocation />);

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
