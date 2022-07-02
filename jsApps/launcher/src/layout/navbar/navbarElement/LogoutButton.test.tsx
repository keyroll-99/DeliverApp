import { fireEvent, render } from "@testing-library/react";
import { Logout } from "service/userService/AuthenticationService";
import LogoutButton from "./LogoutButton";

const mockMutateLogout = jest.fn();

jest.mock("service/userService/AuthenticationService", () => ({
    Logout: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn().mockReturnValue(jest.fn()),
}));

describe("Logout button", () => {
    beforeEach(() => {
        (Logout as jest.MockedFunction<typeof Logout>).mockReturnValue({
            mutateAsync: mockMutateLogout,
            isLoading: false,
        });
    });

    test("should call logout after click button", () => {
        // act
        mockMutateLogout.mockReturnValue({ isSucess: true });
        const component = render(<LogoutButton />);

        const button = component.queryByText("Logout");

        fireEvent(
            button!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        expect(mockMutateLogout).toHaveBeenCalled();
    });
});
