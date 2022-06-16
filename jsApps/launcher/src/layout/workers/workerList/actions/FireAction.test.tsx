import { act, fireEvent, render } from "@testing-library/react";
import { Component } from "react";
import { FireUserAction, GetWorkers } from "service/userService/UserService";
import FireAction from "./FireAction";

jest.mock("service/userService/UserService", () => ({
    FireUserAction: jest.fn(),
    GetWorkers: jest.fn(),
}));

describe("FireAction", () => {
    test("should render Fire action button", () => {
        // arrnage
        (FireUserAction as jest.MockedFunction<typeof FireUserAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
            isLoading: false,
            refresh: jest.fn(),
        });

        // act
        const component = render(<FireAction userHash="hash" />);

        // assert
        expect(component).toMatchSnapshot();
    });

    test("should call mutationFire after click button", async () => {
        // arrange
        const mockMutation = jest.fn().mockReturnValue({ isSuccess: true });
        const mockRefresh = jest.fn();
        (FireUserAction as jest.MockedFunction<typeof FireUserAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: mockMutation,
        });
        (GetWorkers as jest.MockedFunction<typeof GetWorkers>).mockReturnValue({
            isLoading: false,
            refresh: mockRefresh,
        });

        // act
        const component = render(<FireAction userHash="user-hash" />);
        const button = component.container.querySelector(".MuiLoadingButton-root");

        await act(async () => {
            await fireEvent(
                button!,
                new MouseEvent("click", {
                    bubbles: true,
                    cancelable: true,
                })
            );
        });

        // assert
        expect(mockMutation).toBeCalled();
        expect(mockRefresh).toBeCalled();
    });
});
