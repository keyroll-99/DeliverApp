FROM node:16 as development
ENV NODE_ENV development

# utils
WORKDIR /app/utils

COPY ./utils/package.json .

RUN npm install

COPY ./utils .


# launcher
WORKDIR /app/launcher

COPY ./launcher/package.json .

RUN npm install

COPY ./launcher .

# run app

EXPOSE 3000

WORKDIR /app/utils

RUN node config-init.js dev ../launcher

WORKDIR /app/launcher

CMD ["npm", "run", "start:docker"]

FROM node:16 as builder

ENV NODE_ENV=production

# utils
WORKDIR /app/utils

COPY ./utils/package.json .

RUN npm install

COPY ./utils .

# launcher
WORKDIR /app/launcher

COPY ./launcher/package.json .

RUN npm install

COPY ./launcher .

# build
WORKDIR /app/utils

RUN node config-init.js prod ../launcher

WORKDIR /app/launcher

RUN npm run build

FROM nginx:1.21.0-alpine as production

ENV NODE_ENV=production

COPY --from=builder /app/launcher/build /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]