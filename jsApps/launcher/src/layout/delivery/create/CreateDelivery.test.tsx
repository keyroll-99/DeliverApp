import { render } from "@testing-library/react";
import { CreateDeliveryAction } from "service/deliveryService/DeliveryService";
import CreateDelivery from "./CreateDelivery";

jest.mock("../_share/DeliverForm", () => () => <div>Delivery form</div>);
jest.mock("service/deliveryService/DeliveryService", () => ({
    CreateDeliveryAction: jest.fn(),
}));

describe("Create Delivery", () => {
    test("should render form", () => {
        // arrange
        (CreateDeliveryAction as jest.MockedFunction<typeof CreateDeliveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<CreateDelivery />);

        // assert
        expect(component.queryByText("Delivery form")).toBeInTheDocument();
    });
});
