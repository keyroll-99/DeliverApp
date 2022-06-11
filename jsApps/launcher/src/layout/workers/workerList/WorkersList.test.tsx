import { render } from "@testing-library/react";
import UserResponse from "service/userService/models/UserResponse";
import { GetWorkers } from "service/userService/UserService";
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

jest.mock("service/companyService/WorkersServices", () => ({
    GetWorkers: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

test("should render dataGrid when fetch data with success", () => {
    // arrange
    (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
        isLoading: false,
        data: [mockUserResponse],
        isSuccess: true,
    });

    // act
    const { queryByText } = render(<WorkersList />);

    // assert
    expect(queryByText("dataGrid")).toBeInTheDocument();
});

test("should render loader when fetching data ", () => {
    // arrange
    (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
        isLoading: true,
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
        isSuccess: false,
    });

    // act
    const { queryByText } = render(<WorkersList />);

    // assert
    expect(queryByText("something went wrong")).toBeInTheDocument();
});
