import { fireEvent, render, waitFor } from "@testing-library/react";
import UserResponse from "service/userService/models/UserResponse";
import { UpdateUserAction } from "service/userService/UserService";
import { BaseResponse } from "service/_core/Models";
import UpdateUser from "./UpdateUser";

const mockMute = jest.fn();

jest.mock("service/userService/UserService", () => ({
    UpdateUserAction: jest.fn(),
}));

describe("UpdateUser", () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("should render form with value form props", async () => {
        // act
        const component = render(
            <UpdateUser
                userData={{
                    email: "email",
                    name: "name",
                    surname: "surname",
                    userHash: "userHash",
                    phoneNumber: "phoneNumber",
                }}
            />
        );

        // assert
        await waitFor(async () => {
            expect((await component.findByLabelText("Email")).getAttribute("value")).toBe("email");
            expect((await component.findByLabelText("Name")).getAttribute("value")).toBe("name");
            expect((await component.findByLabelText("Surname")).getAttribute("value")).toBe("surname");
            expect((await component.findByLabelText("Phone number")).getAttribute("value")).toBe("phoneNumber");
        });
    });

    test("should show success snackbar after success update", async () => {
        (UpdateUserAction as jest.MockedFunction<typeof UpdateUserAction>).mockReturnValue({
            data: { isSuccess: true, error: "" },
            isLoading: false,
            mutateAsync: mockMute.mockReturnValue({ isSuccess: true, error: "" } as BaseResponse<UserResponse>),
        });

        // act
        const component = render(
            <UpdateUser
                userData={{
                    email: "email",
                    name: "name",
                    surname: "surname",
                    userHash: "userHash",
                    phoneNumber: "phoneNumber",
                }}
            />
        );

        const submitButton = (await component.findAllByText("Change data")).filter(
            (x) => x.getAttribute("type") === "button"
        )[0];

        fireEvent(
            submitButton!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        await waitFor(() => {
            expect(mockMute).toBeCalled();
            const successSnackbar = component.queryByText("The change of data was successful");
            expect(successSnackbar).toBeInTheDocument();
        });
    });

    test("should show error snackbar after faild update", async () => {
        (UpdateUserAction as jest.MockedFunction<typeof UpdateUserAction>).mockReturnValue({
            data: { isSuccess: true, error: "" },
            isLoading: false,
            mutateAsync: mockMute.mockReturnValue({
                isSuccess: false,
                error: "simple-error",
            } as BaseResponse<UserResponse>),
        });

        // act
        const component = render(
            <UpdateUser
                userData={{
                    email: "email",
                    name: "name",
                    surname: "surname",
                    userHash: "userHash",
                    phoneNumber: "phoneNumber",
                }}
            />
        );

        const submitButton = (await component.findAllByText("Change data")).filter(
            (x) => x.getAttribute("type") === "button"
        )[0];

        fireEvent(
            submitButton!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        await waitFor(() => {
            expect(mockMute).toBeCalled();
            const successSnackbar = component.queryByText("simple-error");
            expect(successSnackbar).toBeInTheDocument();
        });
    });
});
