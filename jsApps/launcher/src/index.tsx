import React, { Suspense } from "react";
import ReactDOM from "react-dom";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import { store, StoreContext } from "./stores/Store";
import { BrowserRouter } from "react-router-dom";
import { LoadConfig } from "./utils/_core/Config";
import "./assets/index.scss";
import { QueryClient, QueryClientProvider } from "react-query";

const queryClient = new QueryClient();

fetch(`./config.json`)
    .then((resp) => resp.json())
    .then((data) => {
        LoadConfig(data);
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
    });

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
