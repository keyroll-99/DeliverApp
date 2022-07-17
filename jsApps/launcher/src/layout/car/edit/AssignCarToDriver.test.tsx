import { render } from "@testing-library/react";
import { AssingUserToCarAction } from "service/carService/CarService";
import { GetDriverAction } from "service/userService/UserService";
import AssignCarToDriver from "./AssignCarToDriver";

jest.mock("service/userService/UserService", () => ({
    GetDriverAction: jest.fn(),
}));

jest.mock("service/carService/CarService", () => ({
    AssingUserToCarAction: jest.fn(),
}));

describe(AssignCarToDriver, () => {
    test("should match to snapshot", () => {
        (GetDriverAction as jest.MockedFunction<typeof GetDriverAction>).mockReturnValue({
            isLoading: false,
            data: [
                {
                    hash: "test",
                    companyHash: "",
                    companyName: "",
                    email: "",
                    name: "",
                    roles: [],
                    surname: "",
                    username: "",
                },
            ],
            isSuccess: true,
        });

        (AssingUserToCarAction as jest.MockedFunction<typeof AssingUserToCarAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        const compoent = render(<AssignCarToDriver carHash="asd" />);

        expect(compoent).toMatchSnapshot();
    });
});
