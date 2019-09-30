var prerendering = require('aspnet-prerendering');
import * as React from 'react';
import { renderToString } from 'react-dom/server';
import App from "./app";
import { renderStatic } from '@uifabric/merge-styles/lib/server';

let { html, css } = renderStatic(() => {
  return renderToString(<App />);
});
var doc = `
    <!DOCTYPE html>
    <html lang="en">
    <head>
    <meta charset="UTF-8">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>我的Exchage邮箱-四川路桥</title>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        body {
            background-color: "#fff"
        }
    </style>
    <style>${css}</style>
    </head>
    <body>
          <div id="app">
            ${html}
          </div>
          <script src="/js/client.js"></script>
    </body>
    </html>
`;
console.log(doc);