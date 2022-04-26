import { fireEvent, render } from "@testing-library/react";
import TransferListSelectAll from "./TransferListSelectAll";

describe("TransferListSelectAll", () => {
    test("should render checkbox and call on click after click", () => {
        // arrange
        const onClick = jest.fn();

        // act
        const component = render(
            <TransferListSelectAll isChecked={false} isDisabled={false} onClick={onClick} isIndeterminate={true} />
        );
        const checkbox = component.container.querySelector("[type='checkbox']");
        fireEvent(checkbox!, new MouseEvent("click", { bubbles: true, cancelable: true }));

        // assert
        expect(checkbox).toBeInTheDocument();
        expect(onClick).toBeCalled();
    });
});
