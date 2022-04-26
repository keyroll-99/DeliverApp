import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import { store, StoreContext } from "./stores/Store";
import { BrowserRouter } from "react-router-dom";
import { LoadConfig } from "./utils/_core/Config";
import "./assets/index.scss";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";
import SuccessSnackbar from "components/snackbar/SuccessSnackbar";

const queryClient = new QueryClient();

ReactDOM.render(
    <React.StrictMode>
        <StoreContext.Provider value={store}>
            <QueryClientProvider client={queryClient}>
                <BrowserRouter>
                    <App />
                </BrowserRouter>
            </QueryClientProvider>
        </StoreContext.Provider>
    </React.StrictMode>,
    document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
