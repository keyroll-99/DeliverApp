import { render } from "@testing-library/react";
import {
    AssignUserToCompanyAction,
    CreateCompanyAction,
    GetCompaniesAction,
} from "service/companyService/CompanyServices";
import CompanyPanel from "./CompanyPanel";

jest.mock("service/companyService/CompanyServices", () => ({
    GetCompaniesAction: jest.fn(),
    CreateCompanyAction: jest.fn(),
    AssignUserToCompanyAction: jest.fn(),
}));

describe("CompanyPanel", () => {
    beforeEach(() => {
        (CreateCompanyAction as jest.MockedFunction<typeof CreateCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn(),
        });

        (AssignUserToCompanyAction as jest.MockedFunction<typeof AssignUserToCompanyAction>).mockReturnValue({
            isLoading: false,
        });
    });

    test.each([{ isLoading: false }, { isLoading: true }])("should render components", ({ isLoading }) => {
        // arrange
        (GetCompaniesAction as jest.MockedFunction<typeof GetCompaniesAction>).mockReturnValue({
            isLoading: isLoading,
            data: [],
            refresh: jest.fn(),
        });

        // act
        const compoent = render(<CompanyPanel />);

        // assert
        expect(compoent).toMatchSnapshot();
    });
});
