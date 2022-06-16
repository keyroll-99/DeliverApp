import { render } from "@testing-library/react";
import { CreateCompanyAction } from "service/companyService/CompanyServices";
import CreateCompany from "./CreateCompany";

jest.mock("service/companyService/CompanyServices", () => ({
    CreateCompanyAction: jest.fn(),
}));

describe("CreateCompany", () => {
    test("Should render create company", () => {
        // arrnage
        (CreateCompanyAction as jest.MockedFunction<typeof CreateCompanyAction>).mockReturnValue({
            isLoading: false,
            mutateAsync: jest.fn().mockReturnValue({ isSuccess: true }),
        });

        // act
        const component = render(<CreateCompany refreshCompaniesList={jest.fn()} />);

        // assert
        expect(component).toMatchSnapshot();
    });
});
