# Deliver App
It's app to managment your car fleet. You can easly do staff like 
* Add new workes
* Add new locations
* Add new cars
* Add new delivery

## Requirements to run it on production
* Docker
* Docker-compose

## Requirements to develop this app
* dotnet 6
* node 16.15
* mmsql (you can easliy start your local server by commend `docker-compose up sqlserver`)

## How run it on production
* In the secret folder you have to add 4 fille
    - ConnectionStrings__DefaultConnection which keep connection string to database
    
    - JwtSettings__Secret which keep jwt secret

    - Mail__Login which keep a login to your mailbox

    - Mail_Password which keep a password to your mailbox

* In nginx.conf file change server name to your domain name

* After that run production by commend `docker-compose up --build`
<br />
<br />
<hr />
MIT License
<br  />
Author: Karol Kaźmierczak
<hr>
