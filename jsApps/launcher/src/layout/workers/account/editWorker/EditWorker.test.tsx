import { render } from "@testing-library/react";
import { useParams } from "react-router-dom";
import User from "service/userService/models/User";
import { GetUser } from "service/userService/UserService";
import { UseStore } from "stores/Store";
import UserStore from "stores/UserStore";
import EditWorker, { urlParam } from "./EditWorker";

const mockUser = {
    companyHash: "companyHash",
    expireDate: new Date(),
    hash: "currentUserHash",
    jwt: "jwt",
    name: "name",
    roles: ["test"],
    surname: "surname",
    username: "usermame",
} as User;

const mockUserStore = {
    getUser: mockUser,
} as UserStore;

jest.mock("service/userService/UserService", () => ({
    GetUser: jest.fn(),
    UpdateAction: jest.fn(),
    UpdateUserAction: jest.fn(),
}));

jest.mock("service/userService/AccountService", () => ({
    ChangePasswordAction: jest.fn(),
}));

jest.mock("stores/Store", () => ({
    UseStore: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    ...jest.requireActual("react-router-dom"),
    useParams: jest.fn(),
}));

describe("EditWorker", () => {
    beforeEach(() => {
        jest.clearAllMocks();
        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({ userHash: "hash" } as urlParam);
        (UseStore as jest.MockedFunction<typeof UseStore>).mockReturnValue({ userStore: mockUserStore });
    });

    test("should render loader while downloading user", () => {
        // arrange
        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({ isLoading: true });

        // act
        const component = render(<EditWorker />);

        // assert
        expect(component.container.querySelector(".MuiCircularProgress-circle")).toBeInTheDocument();
    });

    test("should render only UpdateUser when param hash is not same as login user", () => {
        // arrange
        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: {
                hash: "hash",
                companyHash: "companyhash",
                companyName: "name",
                email: "email",
                name: "name",
                roles: [],
                surname: "surname",
                username: "username",
            },
        });

        // act
        const component = render(<EditWorker />);

        // assert
        expect(component.container.querySelector(".MuiCircularProgress-circle")).not.toBeInTheDocument();
        expect(component.queryByText("Name")).toBeInTheDocument();
        expect(component.queryByText("Surname")).toBeInTheDocument();
        expect(component.queryByText("Email")).toBeInTheDocument();
        expect(component.queryByText("current password")).not.toBeInTheDocument();
        expect(component.queryByText("new password")).not.toBeInTheDocument();
    });

    test("should render only UpdateUser when param hash is not same as login user", () => {
        // arrange
        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({
            userHash: "currentUserHash",
        } as urlParam);

        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: {
                hash: "currentUserHash",
                companyHash: "companyhash",
                companyName: "name",
                email: "email",
                name: "name",
                roles: [],
                surname: "surname",
                username: "username",
            },
        });

        // act
        const component = render(<EditWorker />);

        // assert
        expect(component.container.querySelector(".MuiCircularProgress-circle")).not.toBeInTheDocument();
        expect(component.queryByText("Name")).toBeInTheDocument();
        expect(component.queryByText("Surname")).toBeInTheDocument();
        expect(component.queryByText("Email")).toBeInTheDocument();
        expect(component.queryByText("current password")).toBeInTheDocument();
        expect(component.queryByText("new password")).toBeInTheDocument();
    });
});
