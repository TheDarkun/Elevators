#!/bin/bash
container="elevators-mysql"
sqlfile="alldb.sql"
docker cp $sqlfile $container:/$sqlfile
docker exec -it $container mysql -pexample -e "source /$sqlfile"
