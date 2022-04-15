import { render } from "@testing-library/react";
import { MutationProcessing } from "../../service/_core/Models";
import LoginPage from "./LoginPage";

const mockMutationProcessing = {
    isLoading: false,
    mutate: jest.fn(),
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

jest.mock("../../service/userService/UserService", () => ({
    Login: () => jest.fn().mockRejectedValue(mockMutationProcessing),
}));

test("should render login page is not loading", () => {
    // act
    const component = render(<LoginPage />);

    // arrange
    console.log(component.debug());
    expect(component.queryByText("Container")).toBeInTheDocument();
});

test("should not render login page is  loading", () => {
    //arrange
    mockMutationProcessing.isLoading = true;

    // act
    const component = render(<LoginPage />);

    // arrange
    console.log(component.debug());
    expect(component.queryByText("Container")).toBeInTheDocument();
});
