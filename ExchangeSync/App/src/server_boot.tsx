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

var dataWarpper = JSON.parse(new Buffer(detailBase64, 'base64').toString());
var data = dataWarpper.data;
var user = dataWarpper.user;
var wechat = dataWarpper.user.wechat;
var script = "";
if (wechat) {
  script = "<script src=\"/js/client-wechat.js\"></script>";
} else {
  script = "<script src=\"/js/client.js\"></script>";
}


const context = { data: data, user: user } as any;
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
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <meta name="theme-color" content="#0078d4" />
    <link href="/css/fabric.min.css" rel="stylesheet" />
    <script src="https://cdn.bootcss.com/vConsole/3.2.2/vconsole.min.js"></script>
    <script>
        var vConsole = new VConsole();
        console.log('hello world')
    </script>
    <script>
      window.data=${JSON.stringify(data)};
      window.user=${JSON.stringify(user)};
    </script>
    <link rel="stylesheet" href="/css/zx-editor.min.css">
    <link rel="stylesheet" href="/js/styles.css">
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        body {
            background-color: white;
        }
        .kz .ms-DatePicker-table{
          width: 100%;
      }
      .kz .ms-DatePicker-weekday{
          width: 12%;
      }
      
      .kz .ms-DatePicker-holder{
          width: 98%;
      }
      .kz .ms-DatePicker-optionGrid{ width:100%}
      .kz .ms-DatePicker-monthPicker{width:100%}
      .kz .ms-DatePicker-monthOption{width:16%}
    </style>
    <style>${css}</style>
    </head>
    <body>
          <div id="app-react">${html}</div>
          <script src="/js/zx-editor.min.js"></script>
          ${script}
    </body>
    </html>
`;
console.log(doc);