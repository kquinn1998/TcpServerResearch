docker build .
docker run -p 127.0.0.1:13000:13000 $(docker images | awk '{print $1}' | awk 'NR==2')