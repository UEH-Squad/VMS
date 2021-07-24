const path = require("path");

module.exports = {
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/js'),
        filename: "vms.js",
        library: "vms"
    },
    watch: true,
    watchOptions: {
        aggregateTimeout: 500,
        ignored: /node_modules/,
    },
};