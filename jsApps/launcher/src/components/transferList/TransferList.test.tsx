import { render } from "@testing-library/react";
import TransferList from "./TransferList";

describe("TransferList", () => {
    test("checked input should have checked atribute", () => {
        // act
        const component = render(
            <TransferList
                checkeds={[1]}
                elements={[
                    { key: 1, value: "testVal" },
                    { key: 2, value: "testVal2" },
                ]}
                title="test"
                toggleChoose={jest.fn()}
            />
        );

        // arrange
        const checked = component.container.querySelectorAll("input[checked='']");
        expect(checked.length).toBe(1);
    });

    test("should be one extra check input if all input is checked", () => {
        // act
        const component = render(
            <TransferList
                checkeds={[1, 2]}
                elements={[
                    { key: 1, value: "testVal" },
                    { key: 2, value: "testVal2" },
                ]}
                title="test"
                toggleChoose={jest.fn()}
            />
        );

        // arrange
        const checked = component.container.querySelectorAll("input[checked='']");
        expect(checked.length).toBe(3);
    });
});
