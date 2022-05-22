import { render } from "@testing-library/react";
import { useParams } from "react-router-dom";
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

describe("EditDelivery", () => {
    test("should render form", () => {
        // arrange
        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({ deliveryHash: "hash" } as urlParam);
        (GetDeliveryByHash as jest.MockedFunction<typeof GetDeliveryByHash>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
        });

        // act
        const component = render(<EditDelivery />);

        // assert
        expect(component.queryByText("Change delivery")).toBeInTheDocument();
        expect(component.queryByText("update delivery")).toBeInTheDocument();
    });
});
