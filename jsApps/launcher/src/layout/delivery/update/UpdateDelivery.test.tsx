import { render } from "@testing-library/react";
import { CreateDeliveryAction, UpdateDeliveryAction } from "service/deliveryService/DeliveryService";
import Delivery from "service/deliveryService/models/Delivery";
import Location from "service/location/models/Location";
import UpdateDelivery from "./UpdateDelivery";

jest.mock("../_share/DeliverForm", () => () => <div>Delivery form</div>);
jest.mock("service/deliveryService/DeliveryService", () => ({
    UpdateDeliveryAction: jest.fn(),
}));

const mockDelivery = {
    endDate: new Date(),
    from: {
        city: "c",
        country: "c",
        email: "e",
        hash: "hash",
        no: "no",
        phoneNumber: "55555555",
        postalCode: "pc",
        region: "region",
        street: "street",
    },
    to: {
        city: "c",
        country: "c",
        email: "e",
        hash: "hash",
        no: "no",
        phoneNumber: "55555555",
        postalCode: "pc",
        region: "region",
        street: "street",
    },
    hash: "hash",
    name: "name",
    startDate: new Date(),
    status: 1,
} as Delivery;

describe("Create Delivery", () => {
    test("should render form", () => {
        // arrange
        (UpdateDeliveryAction as jest.MockedFunction<typeof CreateDeliveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<UpdateDelivery delivery={mockDelivery} />);

        // assert
        expect(component.queryByText("Delivery form")).toBeInTheDocument();
    });
});
