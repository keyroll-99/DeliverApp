import { render } from "@testing-library/react";
import Select from "./Select";

describe("Select", () => {
    test("should redner select", () => {
        // act
        const compoent = render(
            <Select
                baseClass="base"
                label="label"
                setValue={jest.fn()}
                value={"t"}
                values={[{ key: "t", value: "val" }]}
            />
        );

        // assert
        expect(compoent).toMatchSnapshot();
    });
});
