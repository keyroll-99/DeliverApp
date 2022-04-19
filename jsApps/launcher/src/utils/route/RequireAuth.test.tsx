import { render } from "@testing-library/react";
import RequireAuth from "./RequireAuth";
import { RefreshToken } from "../../service/userService/AuthenticationService";
import { useNavigate } from "react-router-dom";

let mockLoggedIn = true;

jest.mock("../../stores/Store", () => ({
    UseStore: () => ({ userStore: { getIsLogged: mockLoggedIn } }),
}));

jest.mock("react-router-dom", () => ({ useNavigate: jest.fn() }));

jest.mock("../../layout/navbar/Navbar", () => () => <div>navbar</div>);

jest.mock("../../service/userService/AuthenticationService", () => ({
    RefreshToken: jest.fn(),
}));

beforeEach(() => {
    mockLoggedIn = true;
});

test("if user is loggin should redner navbar and child", () => {
    // assert
    (RefreshToken as jest.MockedFunction<typeof RefreshToken>).mockReturnValue({
        isLoading: false,
        isSuccess: true,
    });

    // act
    const { queryByText } = render(
        <RequireAuth>
            <div>child</div>
        </RequireAuth>
    );

    // assert
    expect(queryByText("navbar")).toBeInTheDocument();
    expect(queryByText("child")).toBeInTheDocument();
});

test("if user is not logged in should not render child", () => {
    // assert
    (RefreshToken as jest.MockedFunction<typeof RefreshToken>).mockReturnValue({
        isLoading: false,
        isSuccess: false,
    });
    const mockUseNavigate = jest.fn();
    (useNavigate as jest.MockedFunction<typeof useNavigate>).mockReturnValue(mockUseNavigate);

    // arrange
    mockLoggedIn = false;

    // act
    const { queryByText } = render(
        <RequireAuth>
            <div>child</div>
        </RequireAuth>
    );

    // assert
    expect(queryByText("navbar")).not.toBeInTheDocument();
    expect(queryByText("child")).not.toBeInTheDocument();
    expect(mockUseNavigate).toBeCalled();
});
