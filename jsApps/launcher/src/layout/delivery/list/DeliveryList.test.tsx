import { render } from "@testing-library/react";
import { GetDeliveryList } from "service/deliveryService/DeliveryService";
import DeliveryList from "./DeliveryList";

jest.mock("@mui/x-data-grid", () => ({
    DataGrid: () => <div>dataGrid</div>,
}));

jest.mock("service/deliveryService/DeliveryService", () => ({
    GetDeliveryList: jest.fn(),
}));

jest.mock("@mui/material", () => ({
    CircularProgress: () => <div>loader</div>,
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn().mockReturnValue(jest.fn()),
}));

describe("DeliveryList", () => {
    test("should show list", () => {
        // arragne
        (GetDeliveryList as jest.MockedFunction<typeof GetDeliveryList>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
        });

        // act
        const component = render(<DeliveryList />);

        // assert
        expect(component.queryByText("dataGrid")).toBeInTheDocument();
    });

    test("should show loader", () => {
        // arragne
        (GetDeliveryList as jest.MockedFunction<typeof GetDeliveryList>).mockReturnValue({
            isLoading: true,
            isSuccess: true,
        });

        // act
        const component = render(<DeliveryList />);

        // assert
        expect(component.queryByText("dataGrid")).not.toBeInTheDocument();
        expect(component.queryByText("loader")).toBeInTheDocument();
    });
});
