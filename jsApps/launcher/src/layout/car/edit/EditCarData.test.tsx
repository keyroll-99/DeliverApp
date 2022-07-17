import { render } from "@testing-library/react";
import { UpdateCarAction } from "service/carService/CarService";
import EditCarData from "./EditCarData";

jest.mock("service/carService/CarService", () => ({
    UpdateCarAction: jest.fn(),
}));

describe(EditCarData, () => {
    test("should match to snapshot", () => {
        (UpdateCarAction as jest.MockedFunction<typeof UpdateCarAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        const component = render(
            <EditCarData car={{ brand: "b", hash: "h", model: "m", registrationNumber: "tn", vin: "v" }} />
        );

        expect(component).toMatchSnapshot();
    });
});
