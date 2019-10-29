const path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    //mode: "development",
    mode: "production",
    entry: {
        server: "./app/src/server_boot.tsx"
    },
    output: {
        libraryTarget: 'commonjs',
        filename: "[name].js",
        path: path.resolve(__dirname, './')
    },
    devtool: "inline-source-map",
    target: "node",
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
        new ExtractTextPlugin("styles.css")
    ]
};