import axios, { AxiosError, AxiosRequestHeaders } from "axios";
import { useQuery } from "react-query";
import { UseStore } from "../../stores/Store";
import Endpoints from "../../utils/axios/Endpoints";
import GetHeader from "../../utils/axios/GetHeader";
import Config from "../../utils/_core/Config";
import UserResponse from "../userService/models/UserResponse";
import { BaseResponse, FetchProcessing } from "../_core/Models";
