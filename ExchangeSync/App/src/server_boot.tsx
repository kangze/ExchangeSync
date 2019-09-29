var prerendering = require('aspnet-prerendering');
import * as React from 'react';
import { renderToString } from 'react-dom/server';
import App from "./app";
import { configureLoadStyles } from '@microsoft/load-themed-styles';
let _allStyles = '';

configureLoadStyles((styles: string) => {
  _allStyles += styles;
});

console.log("test");

export default  prerendering.createServerRenderer(function(params:any) {
    return new Promise(function (resolve, reject) {
        const html=renderToString(<App/>);
        
        resolve({ html: html,globals:{text:_allStyles} });
    });
});