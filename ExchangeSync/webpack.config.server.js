const path = require('path');

module.exports = {
    mode: "development",
    entry: {
        server: "./app/src/server_boot.tsx"
    },
    output: {
        libraryTarget: 'commonjs',
        filename: "[name].js",
        path: path.resolve(__dirname, './wwwroot/js/')
    },
    devtool: "inline-source-map",
    target:"node",
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
    }
};