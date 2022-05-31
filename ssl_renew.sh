#!/bin/bash

/usr/local/bin/docker-compose -f /root/sites/$1/repository/pillow/docker-compose.yml run certbot renew  \
&& /usr/local/bin/docker-compose -f /root/sites/$1/repository/pillow/docker-compose.yml kill -s SIGHUP reverseproxy
