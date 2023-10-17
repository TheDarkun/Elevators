@echo off
SET "container=elevators-mysql"
SET "sqlfile=alldb.sql"
docker cp %sqlfile% %container%:/%sqlfile%
docker exec -it %container% mysql -pexample -e "source /%sqlfile%"
