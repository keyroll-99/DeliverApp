import { render } from "@testing-library/react";
import Navbar from "./Navbar";

jest.mock("./DesktopNavbar", () => () => <div>desktop</div>);
jest.mock("./MobileNavbar", () => () => <div>mobile</div>);

test("should render mobile if screen have less than 768px", () => {
    // arrange
    window.innerWidth = 500;

    //act
    const { queryByText } = render(<Navbar />);

    // assert
    expect(queryByText("mobile")).toBeInTheDocument();
    expect(queryByText("desktop")).not.toBeInTheDocument();
});

test("should render desktop if screen have more than 768px", () => {
    // arrange
    window.innerWidth = 800;

    //act
    const { queryByText } = render(<Navbar />);

    // assert
    expect(queryByText("mobile")).not.toBeInTheDocument();
    expect(queryByText("desktop")).toBeInTheDocument();
});
