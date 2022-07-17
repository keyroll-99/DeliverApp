import { render } from "@testing-library/react";
import { CreateCarAction } from "service/carService/CarService";
import CreateCar from "./CreateCar";

jest.mock("service/carService/CarService", () => ({
    CreateCarAction: jest.fn(),
}));

describe(CreateCar, () => {
    test("should match to snapshot", () => {
        (CreateCarAction as jest.MockedFunction<typeof CreateCarAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        const component = render(<CreateCar />);

        expect(component).toMatchSnapshot();
    });
});
