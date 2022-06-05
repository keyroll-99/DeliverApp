import { render } from "@testing-library/react";
import Snackbar from "./Snackbar";

describe("Snackbar", () => {
    test("Should render snackbar", () => {
        // act
        const component = render(<Snackbar message={"message"} variant={"success"} setIsOpen={jest.fn()} />);

        // assert
        expect(component).toMatchSnapshot();
    });
});
