import * as React from 'react';
import { renderToString } from 'react-dom/server';
import App from "./app";
import { renderStatic } from '@uifabric/merge-styles/lib/server';
import { Fabric } from 'office-ui-fabric-react/lib/Fabric';
import { StaticRouter, Route } from 'react-router-dom';

import { initializeIcons } from '@uifabric/icons';

initializeIcons("/fonts/");


var location = process.argv[2];
var detailBase64 = process.argv[3];

var detail = new Buffer(detailBase64, 'base64').toString();

const context = { data: JSON.parse(detail) } as any;
let { html, css } = renderStatic(() => {
  return renderToString(
    <StaticRouter location={location} context={context}>
      <Fabric>
        <App />
      </Fabric>
    </StaticRouter>
  );
});
var doc = `
    <!DOCTYPE html>
    <html lang="en">
    <head>
    <title>我的Exchage邮箱-四川路桥</title>
    <meta charset="UTF-8">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;" name="viewport" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <link href="/css/fabric.min.css" rel="stylesheet" />
    <script>
      window.detail=${detail}
    </script>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        body {
            background-color: "#fff";
        }
        
    </style>
    <style>${css}</style>
    </head>
    <body>
          <div id="app">${html}</div>
          <script src="http://localhost:18080/dist/client.js"></script>
    </body>
    </html>
`;
console.log(doc);