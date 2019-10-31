import * as React from "react";
import * as ReactDOM from "react-dom";
import { initializeIcons } from '@uifabric/icons';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Fabric } from 'office-ui-fabric-react/lib/Fabric';

import App from "./app";

import { hot } from 'react-hot-loader';

initializeIcons("/fonts/");

declare var ZxEditor: any;
ReactDOM.render(
    <Router>
        <Fabric styles={{ root: { fontFamily: "微软雅黑" } }}>
            <App />
        </Fabric>
    </Router>
    ,
    document.getElementById("app-react")
);

module.hot.accept();