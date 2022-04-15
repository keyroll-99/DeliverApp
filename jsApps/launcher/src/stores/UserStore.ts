import { makeAutoObservable } from "mobx";
import { MapAuthResponseToUser } from "../service/userService/Mapper";
import AuthResponse from "../service/userService/models/AuthResponse";
import User from "../service/userService/models/User";

class UserStore {
    _user?: User;
    _isLogged: boolean = false;
    _isLoading: boolean = false;
    _error?: string;

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

    setUser(user: AuthResponse) {
        this._user = MapAuthResponseToUser(user);
        this._isLogged = true;
    }
}

export default UserStore;
