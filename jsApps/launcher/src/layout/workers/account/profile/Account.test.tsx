import { render } from "@testing-library/react";
import User from "service/userService/models/User";
import { GetUser } from "service/userService/UserService";
import { UseStore } from "stores/Store";
import UserStore from "stores/UserStore";
import Account from "./Account";

const mockUser = {
    companyHash: "companyHash",
    expireDate: new Date(),
    hash: "hash",
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
}));

jest.mock("stores/Store", () => ({
    UseStore: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

describe("Account", () => {
    beforeEach(() => {
        jest.clearAllMocks();

        (UseStore as jest.MockedFunction<typeof UseStore>).mockReturnValue({
            userStore: mockUserStore,
        });
    });

    test("should render progres when data is loading", () => {
        // arrange
        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({
            isLoading: true,
        });

        // act
        const component = render(<Account />);

        // assert
        expect(component.container.querySelector(".MuiCircularProgress-circle")).toBeInTheDocument();
    });

    test("should show error when api return error", () => {
        // arrange
        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({
            isLoading: false,
            isSuccess: false,
        });

        // act
        const component = render(<Account />);

        // assert
        expect(
            component.queryByText("oops something went wrong. please reload the page and try again")
        ).toBeInTheDocument();
    });

    test("test should show user data when api retun user data", () => {
        // arrange
        (GetUser as jest.MockedFunction<typeof GetUser>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            data: {
                companyName: "company",
                companyHash: "companyHash",
                hash: "userHash",
                name: "carl",
                roles: ["role1", "role2"],
                surname: "surname",
                username: "username",
                email: "email",
            },
        });

        // act
        const component = render(<Account />);

        // assert
        expect(component.queryByText("company")).toBeInTheDocument();
        expect(component.queryByText("carl surname")).toBeInTheDocument();
        expect(component.queryByText("username")).toBeInTheDocument();
        expect(component.queryByText("role1")).toBeInTheDocument();
        expect(component.queryByText("role2")).toBeInTheDocument();
    });
});
