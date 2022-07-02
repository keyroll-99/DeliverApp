import { fireEvent, render } from "@testing-library/react";
import { act } from "react-dom/test-utils";
import { PasswordRecoveryAction } from "service/userService/AccountService";
import { BaseResponse } from "service/_core/Models";
import PasswordRecoveryFormComponent from "./PasswordRecoveryFormComponent";

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

jest.mock("service/userService/AccountService", () => ({
    PasswordRecoveryAction: jest.fn(),
}));

describe(PasswordRecoveryFormComponent, () => {
    test("should match to snapshot", () => {
        // arrange
        (PasswordRecoveryAction as jest.MockedFunction<typeof PasswordRecoveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<PasswordRecoveryFormComponent recoveryKey="key" />);

        // assert
        expect(component).toMatchSnapshot();
    });

    test.each([
        { isSuccess: true, expectMessage: "Password changed" },
        { isSuccess: false, expectMessage: "error" },
    ])("should render notification after submit button", async ({ isSuccess, expectMessage }) => {
        // arrange
        const mockMutateAsync = jest
            .fn()
            .mockReturnValue({ isSuccess: isSuccess, error: expectMessage } as BaseResponse<boolean>);

        (PasswordRecoveryAction as jest.MockedFunction<typeof PasswordRecoveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: mockMutateAsync,
        });

        // act
        const component = render(<PasswordRecoveryFormComponent recoveryKey="key" />);
        await act(async () => {
            const button = component.getByRole("button", { name: "Set new password" });
            fireEvent(
                button!,
                new MouseEvent("click", {
                    bubbles: true,
                    cancelable: true,
                })
            );
        });

        // assert
        const alert = component.getByRole("alert");
        const backToLoginPageButton = component.queryByText("Back to login page");

        expect(alert).toBeInTheDocument();
        expect(mockMutateAsync).toBeCalled();
        if (isSuccess) {
            expect(backToLoginPageButton).toBeInTheDocument();
        } else {
            expect(backToLoginPageButton).not.toBeInTheDocument();
        }
    });
});
