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
