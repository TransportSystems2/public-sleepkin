#!/usr/bin/env sh

export DOLLAR='$'
envsubst < /start/nginx.conf.template > /etc/nginx/conf.d/default.conf
nginx -g "daemon off;"