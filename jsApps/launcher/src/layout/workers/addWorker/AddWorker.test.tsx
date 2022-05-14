import { render } from "@testing-library/react";
import { GetRoles } from "service/userService/RoleService";
import { CreateUser } from "service/userService/UserService";
import AddWorker from "./AddWorker";

jest.mock("service/userService/RoleService", () => ({
    GetRoles: jest.fn(),
}));

jest.mock("service/userService/UserService", () => ({
    CreateUser: jest.fn(),
}));

describe("AddWorker", () => {
    beforeEach(() => {
        (CreateUser as jest.MockedFunction<typeof CreateUser>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });
    });

    test("should render form after load roles", () => {
        // arrange

        (GetRoles as jest.MockedFunction<typeof GetRoles>).mockReturnValue({
            isLoading: false,
            data: [
                { id: 1, name: "role1" },
                { id: 2, name: "role2" },
            ],
        });

        // act
        const component = render(<AddWorker />);

        // assert
        expect(component.queryByText("Name")).toBeInTheDocument();
        expect(component.queryByText("Surname")).toBeInTheDocument();
        expect(component.queryByText("Email")).toBeInTheDocument();
        expect(component.queryByText("Username")).toBeInTheDocument();
        expect(component.queryByText("avaliable role")).toBeInTheDocument();
        expect(component.queryByText("choosed role")).toBeInTheDocument();
        expect(component.queryByText("submit")).toBeInTheDocument();
        expect(component.container.querySelector(".MuiCircularProgress-circle")).not.toBeInTheDocument();
    });

    test("should render loaded during fetch roles", () => {
        // arrange

        (GetRoles as jest.MockedFunction<typeof GetRoles>).mockReturnValue({
            isLoading: true,
            data: [
                { id: 1, name: "role1" },
                { id: 2, name: "role2" },
            ],
        });

        // act
        const component = render(<AddWorker />);

        // assert
        expect(component.queryByText("Name")).not.toBeInTheDocument();
        expect(component.queryByText("Surname")).not.toBeInTheDocument();
        expect(component.queryByText("Email")).not.toBeInTheDocument();
        expect(component.queryByText("Username")).not.toBeInTheDocument();
        expect(component.queryByText("avaliable role")).not.toBeInTheDocument();
        expect(component.queryByText("choosed role")).not.toBeInTheDocument();
        expect(component.queryByText("submit")).not.toBeInTheDocument();
        expect(component.container.querySelector(".MuiCircularProgress-circle")).toBeInTheDocument();
    });
});
