import { fireEvent, render, waitFor } from "@testing-library/react";
import ChangePassword from "./ChangePassword";
import { ChangePasswordAction } from "service/userService/UserService";
import { BaseResponse } from "service/_core/Models";
import UserResponse from "service/userService/models/UserResponse";

const mockMutateAsync = jest.fn();

jest.mock("service/userService/UserService", () => ({
    ChangePasswordAction: jest.fn(),
}));

describe("Change password", () => {
    beforeEach(() => {
        (ChangePasswordAction as jest.MockedFunction<typeof ChangePasswordAction>).mockReturnValue({
            isLoading: false,
            isSuccess: true,
            mutateAsync: mockMutateAsync,
        });
    });

    test("should render form", () => {
        // act
        const component = render(<ChangePassword />);

        // assert
        expect(component.queryByText("current password")).toBeInTheDocument();
        expect(component.queryByText("new password")).toBeInTheDocument();
    });

    test("should call update after click submit and show success snackbar", async () => {
        // arrange
        mockMutateAsync.mockReturnValue({ isSuccess: true } as BaseResponse<UserResponse>);

        // act
        const component = render(<ChangePassword />);

        const submitButton = component.container.querySelector(".change-password-submit");

        fireEvent(
            submitButton!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        await waitFor(() => {
            expect(mockMutateAsync).toBeCalled();
            expect(component.queryByText("Password change")).toBeInTheDocument();
        });
    });
});
