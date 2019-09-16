const path = require('path');
const webpack = require('webpack');//引入webpack

module.exports = {
    mode: "development",
    entry: "./app/src/index.tsx",
    output: {

        filename: "bundle.js",
        publicPath: "http://192.168.18.171:18080/dist/",
        path: path.resolve(__dirname, './wwwroot/js/')
    },
    devtool: "inline-source-map",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin()
    ],

    devServer: {
        contentBase: path.join(__dirname, "./app/dist"),
        headers: {
            'Access-Control-Allow-Origin': '*' //配合服务端开发,需要跨域
        },
        port: 18080,
        host: "192.168.18.171",
        hot: true
    },
}