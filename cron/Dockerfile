FROM alpine:3.7

# Setting up crontab
COPY cron/crontab /tmp/crontab
RUN cat /tmp/crontab > /etc/crontabs/root

RUN apk --no-cache add curl

CMD ["crond", "-f", "-l", "2"]