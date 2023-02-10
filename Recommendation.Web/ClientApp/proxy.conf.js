const {env} = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:11515';

const PROXY_CONFIG = [
  {
    context: [
      "/api/users",
      "/api/reviews",
      "/api/tags",
      "/api/categories",
      "/api/rating",
      "/api/likes",
      "/api/comments",
      "/api/tags",
      "/api/compositions",
      "/comment-hub",
      "/signin-google",
      "/signin-discord"
    ],
    target: target,
    secure: false,
    ws: true,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
