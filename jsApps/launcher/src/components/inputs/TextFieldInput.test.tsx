import { render } from "@testing-library/react";
import TextFieldInput from "./TextFieldInput";

describe("TextFieldInput", () => {
    test("Should render TextFieldInput", () => {
        // act
        const compoent = render(
            <TextFieldInput baseClass={"baseClass"} label={"label"} type={"text"} onChange={jest.fn()} />
        );

        // asset
        expect(compoent).toMatchSnapshot();
    });
});
