const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:5265';

const PROXY_CONFIG = [
  {
    context: [
      "/api/Auth/register",
      "/api/Auth/login",
      "/api/Auth/refresh"
    ],
    target,
    secure: false,
    changeOrigin: true,
    logLevel: "debug"
  }
]

module.exports = PROXY_CONFIG;
