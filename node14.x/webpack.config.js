const path = require('path');
const webpack = require('webpack');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const { ESBuildPlugin } = require('esbuild-loader');
const ZipPlugin = require('zip-webpack-plugin');

module.exports = {
  entry: './src/handler.ts',
  mode: "production",
  target: 'node',
  module: {
    rules: [
      {
        test: /\.ts?$/,
        use: 'ts-loader',
        exclude: /node_modules/
      }
    ]
  },
  resolve: {
    extensions: ['.ts', '.js', '.json']
  },
  externals: [
    'aws-sdk/clients/dynamodb',
  ],
  plugins: [
    new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
    new ESBuildPlugin(),
    new ZipPlugin({
      filename: `index.zip`
    })
  ],
  output: {
    filename: 'index.js',
    path: path.resolve(__dirname, '../resources/'),
    libraryTarget: 'commonjs2'
  }
};
