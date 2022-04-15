import { render } from "@testing-library/react";
import RequireAuth from "./RequireAuth";

let mockLoggedIn = true;

jest.mock("../../stores/Store", () => ({
    UseStore: () => ({ userStore: { getIsLogged: mockLoggedIn } }),
}));

jest.mock("react-router-dom", () => ({ Navigate: jest.fn() }));

jest.mock("../../components/Navbar", () => () => <div>navbar</div>);

beforeEach(() => {
    mockLoggedIn = true;
});

test("if user is loggin should redner navbar and child", () => {
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
});
