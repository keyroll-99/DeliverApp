import { act, fireEvent, render } from "@testing-library/react";
import { ChangeDeliveryStatusAction } from "service/deliveryService/DeliveryService";
import Delivery from "service/deliveryService/models/Delivery";
import { BaseResponse } from "service/_core/Models";
import ChangeDeliveryStatus from "./ChangeDeliveryStatus";

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

jest.mock("service/deliveryService/DeliveryService", () => ({
    ChangeDeliveryStatusAction: jest.fn(),
}));

describe("ChangeDeliveryStatus", () => {
    test("should show form", async () => {
        // arrange
        const mutate = jest.fn().mockReturnValue({ isSuccess: true } as BaseResponse<Delivery>);
        (ChangeDeliveryStatusAction as jest.MockedFunction<typeof ChangeDeliveryStatusAction>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            mutateAsync: mutate,
        });

        // act
        const component = render(<ChangeDeliveryStatus delivery={mockDelivery} />);

        await act(async () => {
            const changeStatus = await component.queryByText("Change status");

            fireEvent(
                changeStatus!,
                new MouseEvent("click", {
                    bubbles: true,
                    cancelable: true,
                })
            );
        });

        // assert
        expect(component!.queryByText("Change delivery status")).toBeInTheDocument();
        expect(component!.queryByText("Status")).toBeInTheDocument();
        expect(component!.queryByText("Change status")).toBeInTheDocument();
        expect(mutate).toBeCalled();
    });
});
