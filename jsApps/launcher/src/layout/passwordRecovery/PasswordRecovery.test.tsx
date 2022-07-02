import { render } from "@testing-library/react";
import { useParams } from "react-router-dom";
import { IsValidRecoveryKeyAction, PasswordRecoveryAction } from "service/userService/AccountService";
import PasswordRecovery from "./PasswordRecovery";

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
    useParams: jest.fn(),
}));

jest.mock("service/userService/AccountService", () => ({
    PasswordRecoveryAction: jest.fn(),
    IsValidRecoveryKeyAction: jest.fn(),
}));

describe(PasswordRecovery, () => {
    beforeEach(() => {
        (PasswordRecoveryAction as jest.MockedFunction<typeof PasswordRecoveryAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        (useParams as jest.MockedFunction<typeof useParams>).mockReturnValue({ recoveryKey: "key" });
    });

    test.each([true, false])("should match to snapshot", (isValid) => {
        // arrange
        (IsValidRecoveryKeyAction as jest.MockedFunction<typeof IsValidRecoveryKeyAction>).mockReturnValue({
            isLoading: false,
            data: isValid,
        });

        // act
        const compoent = render(<PasswordRecovery />);

        // assert
        expect(compoent).toMatchSnapshot();
    });
});
