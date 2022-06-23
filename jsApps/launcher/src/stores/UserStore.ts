import { makeAutoObservable } from "mobx";
import { Permission } from "service/userService/models/Permissions";
import { MapAuthResponseToUser } from "../service/userService/Mapper";
import AuthResponse from "../service/userService/models/AuthResponse";
import User from "../service/userService/models/User";

class UserStore {
    private _user?: User;
    private _permissions?: Permission;
    private _isLogged: boolean = false;
    private _isLoading: boolean = false;
    private _error?: string;

    constructor() {
        makeAutoObservable(this);
    }

    get getUser() {
        return this._user;
    }

    get getIsLoading() {
        return this._isLoading;
    }

    get getIsLogged() {
        return this._isLogged;
    }

    get getError() {
        return this._error;
    }

    get getPermissions() {
        return this._permissions;
    }

    setUser(user: AuthResponse) {
        this._user = MapAuthResponseToUser(user);
        this._isLogged = true;
    }

    setPermissions(permissions?: Permission) {
        this._permissions = permissions;
    }

    logout() {
        this._user = undefined;
        this._isLogged = false;
        this._permissions = undefined;
    }
}

export default UserStore;
