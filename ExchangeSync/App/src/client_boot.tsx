import * as React from "react";
import * as ReactDOM from "react-dom";
import { initializeIcons } from '@uifabric/icons';

import App from "./app";

import { hot } from 'react-hot-loader';

initializeIcons();

ReactDOM.render(
    <App />
    ,
    document.getElementById("app")
);

module.hot.accept();