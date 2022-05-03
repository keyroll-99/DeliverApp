import { fireEvent, render } from "@testing-library/react";
import TransferListGrid from "./TransferListGrid";

describe("TrensferListGrid", () => {
    test("should render transfer list", () => {
        // arrange
        const toAvaliable = jest.fn();
        const toSelected = jest.fn();

        // act
        const component = render(
            <TransferListGrid
                availableItems={[]}
                baseClass="baseClass"
                leftListTitle="left title"
                rightListTitle="right title"
                selectedItems={[]}
                toAvaliable={toAvaliable}
                toSelected={toSelected}
            />
        );

        // assert
        expect(component.queryByText("left title")).toBeInTheDocument();
        expect(component.queryByText("right title")).toBeInTheDocument();
    });

    test("should call toSelected after click button", () => {
        // arrange
        const toAvaliable = jest.fn();
        const toSelected = jest.fn();

        // act
        const component = render(
            <TransferListGrid
                availableItems={[]}
                baseClass="baseClass"
                leftListTitle="left title"
                rightListTitle="right title"
                selectedItems={[]}
                toAvaliable={toAvaliable}
                toSelected={toSelected}
            />
        );

        const container = component.container;
        const toSelectedButton = container.querySelector(".test_to-seleted");

        fireEvent(
            toSelectedButton!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        expect(toSelected).toHaveBeenCalled();
    });

    test("should call toAvaliable after click button", () => {
        // arrange
        const toAvaliable = jest.fn();
        const toSelected = jest.fn();

        // act
        const component = render(
            <TransferListGrid
                availableItems={[]}
                baseClass="baseClass"
                leftListTitle="left title"
                rightListTitle="right title"
                selectedItems={[]}
                toAvaliable={toAvaliable}
                toSelected={toSelected}
            />
        );

        const container = component.container;
        const toAvaliableButton = container.querySelector(".test_to-avaliable");

        fireEvent(
            toAvaliableButton!,
            new MouseEvent("click", {
                bubbles: true,
                cancelable: true,
            })
        );

        // assert
        expect(toAvaliable).toHaveBeenCalled();
    });
});
