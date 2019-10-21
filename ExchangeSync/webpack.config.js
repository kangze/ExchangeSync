const path = require('path');
const webpack = require('webpack');//引入webpack
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    //mode: "development",
    mode:"production",
    entry: {
        client: "./app/src/client_boot.tsx",
    },
    output: {
        filename: "[name].js",
        publicPath: "http://localhost:18080/dist/",
        path: path.resolve(__dirname, './wwwroot/js/')
    },
    //devtool: "inline-source-map",
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
            {

                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: "css-loader"
                })
            }
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new ExtractTextPlugin("styles.css")
    ],

    devServer: {
        contentBase: path.join(__dirname, "./app/dist"),
        headers: {
            'Access-Control-Allow-Origin': '*' //配合服务端开发,需要跨域
        },
        port: 18080,
        host: "localhost",
        hot: true
    },
}