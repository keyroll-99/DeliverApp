import { render } from "@testing-library/react";
import { MutationProcessing } from "../../service/_core/Models";
import PasswordRecoveryInit from "./PasswordRecoveryInit";

const mockMutationProcessing = {
    isLoading: false,
    mutate: jest.fn(),
    mutateAsync: jest.fn(),
} as MutationProcessing<null, null>;

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

jest.mock("@mui/material", () => ({
    Box: () => <div>Box</div>,
    Container: () => <div>Container</div>,
    Button: () => <div>Button</div>,
    TextField: () => <div>TextField</div>,
}));

jest.mock("@mui/lab", () => ({
    LoadingButton: () => <div>loading button</div>,
}));

jest.mock("../../service/userService/AuthenticationService", () => ({
    Login: () => jest.fn().mockRejectedValue(mockMutationProcessing),
}));

test("should render login page is not loading", () => {
    // act
    const component = render(<PasswordRecoveryInit />);

    // arrange
    expect(component.queryByText("Container")).toBeInTheDocument();
});
