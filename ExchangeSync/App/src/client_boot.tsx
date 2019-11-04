import * as React from "react";
import * as ReactDOM from "react-dom";
import { initializeIcons } from '@uifabric/icons';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Fabric } from 'office-ui-fabric-react/lib/Fabric';

declare function require(moduleName: string): any;
const AnimatedRoute = require('react-router-transition').AnimatedRoute;
const spring = require('react-router-transition').spring;

import App from "./app";

import { hot } from 'react-hot-loader';

function mapStyles(styles: any) {
    return {
        opacity: styles.opacity,
        transform: `scale(${styles.scale})`,
    };
}

function bounce(val: any) {
    return spring(val, {
        stiffness: 330,
        damping: 22,
    });
}

const bounceTransition = {
    // start in a transparent, upscaled state
    atEnter: {
      opacity: 0,
      scale: 1.2,
    },
    // leave in a transparent, downscaled state
    atLeave: {
      opacity: bounce(0),
      scale: bounce(0.8),
    },
    // and rest at an opaque, normally-scaled state
    atActive: {
      opacity: bounce(1),
      scale: bounce(1),
    },
  };

initializeIcons("/fonts/");
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