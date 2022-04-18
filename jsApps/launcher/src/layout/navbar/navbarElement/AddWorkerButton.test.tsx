import { render } from "@testing-library/react";
import HasRole from "../../../service/userService/Roles";
import AddWorkerButton from "./AddWorkerButton";

jest.mock("@mui/material", () => ({
    ListItemButton: () => <div>ListItemMock</div>,
}));

jest.mock("../../../service/userService/Roles", () => ({
    __esModule: true, // this property makes it work
    default: jest.fn(),
    HasRole: jest.fn(),
    Roles: ["test1"],
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

test("should render item when user has valid role", () => {
    // arrange
    (HasRole as jest.MockedFunction<typeof HasRole>).mockReturnValue(true);

    // act
    const { queryByText } = render(<AddWorkerButton roles={["test1"]} />);

    // assert
    expect(queryByText("ListItemMock")).toBeInTheDocument();
});

test("should not render item when user has not valid role", () => {
    // arrange
    (HasRole as jest.MockedFunction<typeof HasRole>).mockReturnValue(false);

    // act
    const { queryByText } = render(<AddWorkerButton roles={["test1"]} />);

    // assert
    expect(queryByText("ListItemMock")).not.toBeInTheDocument();
});
