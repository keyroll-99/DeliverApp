import { render } from "@testing-library/react";
import UserResponse from "service/userService/models/UserModels/UserResponse";
import { FireUserAction, GetWorkers } from "service/userService/UserService";
import WorkersList from "./WorkersList";

const mockUserResponse: UserResponse = {
    companyHash: "asda",
    companyName: "just name",
    hash: "hash",
    name: "name",
    roles: ["admin"],
    surname: "surname",
    username: "username",
    email: "email",
};

jest.mock("@mui/x-data-grid", () => ({
    DataGrid: () => <div>dataGrid</div>,
}));

jest.mock("@mui/material", () => ({
    CircularProgress: () => <div>loader</div>,
}));

jest.mock("service/userService/UserService", () => ({
    GetWorkers: jest.fn(),
    FireUserAction: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

jest.mock("@mui/lab", () => ({
    LoadingButton: () => <>loading buttons</>,
}));

describe("Workers list", () => {
    beforeEach(() => {
        (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
            isLoading: false,
            data: [mockUserResponse],
            isSuccess: true,
        });

        (FireUserAction as jest.MockedFunction<typeof FireUserAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });
    });

    test("should render dataGrid when fetch data with success", () => {
        // act
        const { queryByText } = render(<WorkersList />);

        // assert
        expect(queryByText("dataGrid")).toBeInTheDocument();
    });

    test("should render loader when fetching data ", () => {
        // arragne
        (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
            isLoading: true,
            data: [mockUserResponse],
            isSuccess: true,
        });

        // act
        const { queryByText } = render(<WorkersList />);

        // assert
        expect(queryByText("loader")).toBeInTheDocument();
    });

    test("should render error when  error occurred while fetch", () => {
        // arrange
        (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
            isLoading: false,
            data: [mockUserResponse],
            isSuccess: false,
        });

        // act
        const { queryByText } = render(<WorkersList />);

        // assert
        expect(queryByText("something went wrong")).toBeInTheDocument();
    });
});
