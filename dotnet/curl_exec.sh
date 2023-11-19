while true
do
  curl http://localhost:8080/rolldice/`head /dev/urandom |md5sum | head -c 7`
  sleep 1
done

