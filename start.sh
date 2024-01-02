#!/bin/bash
docker-compose up --detach
(trap "docker-compose down & kill 0" SIGINT; source ./Scripts/auction.sh & source ./Scripts/search.sh & source ./Scripts/identity.sh & source ./Scripts/gateway.sh)