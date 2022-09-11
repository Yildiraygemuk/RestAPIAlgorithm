# RestAPIAlgorithm <br></br>
In this project, I used caching with Redis so that the requests do not stop when the server is stopped.<br></br>
Therefore, with docker, you must first download the redis image with the following commands and then install it from the 6379 port.<br></br>
```
docker run --name some-redis -d redis
```
```
docker run -p 6379:6379 --name some-redis -d redis
```

There is a job in the background that works once a minute and sends a post request with get and random data.<br></br>
It writes its output to the log file in the project directory. If you want to make a request manually, you can request from port 5000 as follows.<br></br>
GET
```
https://localhost:5000/api/customer
```
POST
```
https://localhost:5000/api/customer
```
