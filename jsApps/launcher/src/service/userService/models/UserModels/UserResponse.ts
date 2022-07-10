import BaseUserResponse from "./BaseUserResponse";

export default interface UserResponse extends BaseUserResponse {
    roles: string[];
    companyName: string;
    companyHash: string;
}
