import { render } from "@testing-library/react";
import {
    AssignUserToCompanyAction,
    CreateCompanyAction,
    GetCompaniesAction,
} from "service/companyService/CompanyServices";
import AdminPanel from "./AdminPanel";

jest.mock("service/companyService/CompanyServices", () => ({
    CreateCompanyAction: jest.fn(),
    GetCompaniesAction: jest.fn(),
    AssignUserToCompanyAction: jest.fn(),
}));

describe("AdminPanel", () => {
    beforeEach(() => {
        (CreateCompanyAction as jest.MockedFunction<typeof CreateCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn().mockReturnValue({ isSuccess: true }),
        });

        (GetCompaniesAction as jest.MockedFunction<typeof GetCompaniesAction>).mockReturnValue({
            isLoading: false,
            data: [{ hash: "hash", name: "name", email: "em", phoneNumber: "phone" }],
        });

        (AssignUserToCompanyAction as jest.MockedFunction<typeof AssignUserToCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn().mockReturnValue({ isSuccess: true }),
        });
    });

    test("should match to snapshot", () => {
        // act
        const compoent = render(<AdminPanel />);

        // assert
        expect(compoent).toMatchSnapshot();
    });
});
