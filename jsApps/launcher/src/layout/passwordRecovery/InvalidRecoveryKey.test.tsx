import { render } from "@testing-library/react";
import InvalidRecoveryKey from "./InvalidRecoveryKey";

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

describe(InvalidRecoveryKey, () => {
    test("Should match to snapshot", () => {
        // act
        const component = render(<InvalidRecoveryKey />);

        // assert
        expect(component).toMatchSnapshot();
    });

    test("should render with correct test", () => {
        // act
        const component = render(<InvalidRecoveryKey />);

        // assert
        expect(component.queryByText("Your recovery link is invalid")).toBeInTheDocument();
        expect(component.getByRole("button", { name: "Back to login page" })).toBeInTheDocument();
    });
});
