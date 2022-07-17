import { render } from "@testing-library/react";
import { useParams } from "react-router-dom";
import { AssingCarToDeliveryAction, GetCarsListAction } from "service/carService/CarService";
import { GetDeliveryByHash } from "service/deliveryService/DeliveryService";
import EditDelivery, { urlParam } from "./EditDelivery";

jest.mock("./ChangeDeliveryStatus", () => () => <div>Change delivery</div>);

jest.mock("./UpdateDelivery", () => () => <div>update delivery</div>);

jest.mock("react-router-dom", () => ({
    ...jest.requireActual("react-router-dom"),
    useParams: jest.fn(),
}));

jest.mock("service/deliveryService/DeliveryService", () => ({
    GetDeliveryByHash: jest.fn(),
}));

jest.mock("service/carService/CarService", () => ({
    GetCarsListAction: jest.fn(),
    AssingCarToDeliveryAction: jest.fn(),
}));

describe("EditDelivery", () => {
    test("should render form", () => {
        // arrange
        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({ deliveryHash: "hash" } as urlParam);
        (GetDeliveryByHash as jest.MockedFunction<typeof GetDeliveryByHash>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: {
                hash: "h",
                endDate: new Date(),
                startDate: new Date(),
                status: 1,
                name: "test",
                from: {
                    city: "c",
                    country: "c",
                    email: "e",
                    hash: "s",
                    no: "2",
                    phoneNumber: "ss",
                    postalCode: "2",
                    region: "2",
                    street: "s",
                },
                to: {
                    city: "c",
                    country: "c",
                    email: "e",
                    hash: "s",
                    no: "2",
                    phoneNumber: "ss",
                    postalCode: "2",
                    region: "2",
                    street: "s",
                },
            },
        });

        (GetCarsListAction as jest.MockedFunction<typeof GetCarsListAction>).mockReturnValue({
            isLoading: false,
            data: [{ brand: "b", hash: "h", model: "m", registrationNumber: "rn", vin: "v" }],
            isSuccess: true,
        });

        (AssingCarToDeliveryAction as jest.MockedFunction<typeof AssingCarToDeliveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<EditDelivery />);

        // assert
        expect(component.queryByText("Change delivery")).toBeInTheDocument();
        expect(component.queryByText("update delivery")).toBeInTheDocument();
    });
});
