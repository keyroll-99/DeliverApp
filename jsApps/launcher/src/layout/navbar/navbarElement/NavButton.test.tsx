import { render } from "@testing-library/react";
import { PermisionToActionEnum } from "service/userService/models/Permissions";
import { HasPermission } from "service/userService/Roles";
import NavButton from "./NavButton";

jest.mock("@mui/material", () => ({
    ListItemButton: () => <div>ListItemMock</div>,
}));

jest.mock("service/userService/Roles", () => ({
    HasPermission: jest.fn(),
}));

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

describe("NavButton", () => {
    test("should render item when user has valid role", () => {
        // arrange
        (HasPermission as jest.MockedFunction<typeof HasPermission>).mockReturnValue(true);

        // act
        const { queryByText } = render(
            <NavButton
                text="Text"
                targetLocation="location"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "company" }}
            />
        );

        // assert
        expect(queryByText("ListItemMock")).toBeInTheDocument();
    });

    test("should not render item when user has not valid role", () => {
        // arrange
        (HasPermission as jest.MockedFunction<typeof HasPermission>).mockReturnValue(false);

        // act
        const { queryByText } = render(
            <NavButton
                text="Text"
                targetLocation="location"
                requirePermission={{ permissionAction: PermisionToActionEnum.create, permissionTo: "company" }}
            />
        );

        // assert
        expect(queryByText("ListItemMock")).not.toBeInTheDocument();
    });
});
