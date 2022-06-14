import { render } from "@testing-library/react";
import { FireUserAction } from "service/userService/UserService";
import FireAction from "./FireAction";

jest.mock("service/userService/UserService", () => ({
    FireUserAction: jest.fn(),
}));

describe("FireAction", () => {
    test("should render Fire action button", () => {
        // arrnage
        (FireUserAction as jest.MockedFunction<typeof FireUserAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        // act
        const component = render(<FireAction userHash="hash" />);

        // assert
        expect(component).toMatchSnapshot();
    });
});
