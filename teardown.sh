docker stop tpot*
docker rmi $(docker images -a | grep 'tpot*')