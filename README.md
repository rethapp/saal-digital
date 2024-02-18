This is a sample project for saal-digital interview process.
I started from some boilerplate projects (template projects) and then I:
- fixed some bugs inside them
- refactored them, a little bit
- extended them
- deployed them using docker-compose.yml technique...

First of all, for me it was important to choose the right architecture, while I did not look very much at the UI.
I mostly concentrate my attention to:

- build a modern infrastructure (but not too much, I mean I did not use Aspire.NET as I was thinking in a 1st moment)
- state of the art for data management ( with EFCore )
- resilience ( rabbitmq + retry policy )
- easyness to deploy to remote server ( docker-compose )

It was not easy to put together a set of starting projects, but finally I did it.
My infrastructure is composed of 5 projects:

1. APIs: rest APIs , standard controller-based with rabbitmq integration
2. Razor app, with ugly UI to create weather info (they are sent to a queue and then consumed): used by an "admin"
3. a processor microservice that has a background worker thread that consumes the messages in the queue and insert into the db
4. a Blazor app used by "users" to view the weatherforecast info
5. a gateway, based on YARP: this one is working outside docker but not perfectly inside it: I have to understand why... so now it is not used.
 
Pre-requisites, to manage the source code and deploy the services:
1. have a github account
2. have a docker-hub account
3. have a decent Linux server where to deploy the docker-compose.yml
	--> I had in a first time created a t3.micro Linux server for free on AWS but it was not able to efficiently run the services ( low memory )
	--> I had to buy a t3.medium server on AWS (paid) and configured it, installing docker and docker compose
	--> I also installed nginx but there is a non clear behaviour so I am not able to use it as a reverse proxy ( also YARP not working as expected :-( )

The source code is located here, I have two branches (v2 is the newest, I did not create the pull request, sorry):
https://github.com/rethapp/saal-digital/tree/v2

The applications are available here:
1. admin (to create weatherforecasts): http://51.20.76.93
	- !!!NOTE ---> THERE IS A BUG!!! After you insert a weather forecast, before you can insert another weather forecast, 
				you must MANUALLY reload the page clicking again
				on the top nav menu item: "Add Forecast Information" ( sorry, I had no time to be perfect :-( )
				I was not able to fix this bug in a reasonable time :-(
2. users view: http://51.20.76.93:8082
3. swagger, to directly test the APIs: http://51.20.76.93:8083/swagger

Also, you could access the postgresql db directly on port 5432, if you need (just ask me) and the rabbitmq control panel (port 15672)...
I can also show how I manage the server, using ssh and how I deal with AWS console, in case you ask...
I have written also a very detailed txt file with all what I did, I could show you if you ask...

Because I had not much time, I could not implement all that I wanted to show you about my knowledge / capacities.
I will improve the test project in the next week(s) with these features:

- add gRPC to notify users when a new weather info is added.
- add some other useful middlewares
- add a second database and implement a second migration...
- add a new table "Towns" in the database
	--> update migration...
- alter the table WeatherForecasts to add a foreign key to town.Id
- add a new migration step to insert some towns
- adjust the weather api + towns api, considering also the town fk
- implement the CQRS pattern to separate the add / edit / delete ops (for admins ) and the queries ( mostly for users, see later ) (MediatR pattern)
- add a circuit braker pattern implementation (Polly)
- add testing projects:
	1. unit testing
	2. functional testing
	3. integration testing
- add a redis cache server for queries filtered by towns, for example
- add an identity server to let only admins to add the weather info
	- protect the apis
--> add https ( after nginx or YARP will work as expected )
- add distributed tracing + loggin + metrics
- create a k8s cluster ( Rancher )
- publish to cluster
- transform in pure DDD project with Aspire.NET
- add database replication
- replace direct communication with rabbitmq with the more agnostic Wolverine
- etc...












