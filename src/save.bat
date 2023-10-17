docker exec -it elevators-mysql bash -c "mysqldump -u root -pexample --all-databases > /alldb.sql"
docker cp elevators-mysql:/alldb.sql ./alldb.sql
