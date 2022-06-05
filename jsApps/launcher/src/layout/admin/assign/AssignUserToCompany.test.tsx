import { act, fireEvent, render } from "@testing-library/react";
import { AssignUserToCompanyAction, GetCompaniesAction } from "service/companyService/CompanyServices";
import AssignUserToCompany from "./AssignUserToCompany";

jest.mock("service/companyService/CompanyServices", () => ({
    CreateCompanyAction: jest.fn(),
    GetCompaniesAction: jest.fn(),
    AssignUserToCompanyAction: jest.fn(),
}));

describe("AssignUserToCompany", () => {
    test("should render form", () => {
        const mockMutation = jest.fn().mockReturnValue({ isSuccess: true });

        (GetCompaniesAction as jest.MockedFunction<typeof GetCompaniesAction>).mockReturnValue({
            isLoading: false,
            data: [{ hash: "hash", name: "name", email: "em", phoneNumber: "phone" }],
        });

        (AssignUserToCompanyAction as jest.MockedFunction<typeof AssignUserToCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: mockMutation,
        });

        // act
        const component = render(<AssignUserToCompany />);

        // assert
        expect(component).toMatchSnapshot();
    });

    test("shoudl call mutation after button click", async () => {
        const mockMutation = jest.fn().mockReturnValue({ isSuccess: true });

        (GetCompaniesAction as jest.MockedFunction<typeof GetCompaniesAction>).mockReturnValue({
            isLoading: false,
            data: [{ hash: "hash", name: "name", email: "em", phoneNumber: "phone" }],
        });

        (AssignUserToCompanyAction as jest.MockedFunction<typeof AssignUserToCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: mockMutation,
        });

        // act
        await act(async () => {
            const component = render(<AssignUserToCompany />);

            const button = await component.findByText("Assign");

            fireEvent(
                button!,
                new MouseEvent("click", {
                    bubbles: true,
                    cancelable: true,
                })
            );
        });

        // assert
        expect(mockMutation).toBeCalled();
    });
});
