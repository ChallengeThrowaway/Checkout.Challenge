# Installation/Running

The payment gateway, SQL server data store, and acquiring bank mock have all been containerised using docker. Running this solution it *should* just be as simple as cloning the project and running the following from the root directory (please ensure ports 1433, 5000, and 6000 are all clear)
```
docker-compose up
```
Once all three containers are running you can make requests to the API from your favorite REST client. The API contract can be found by going to http://localhost:5000/swagger

To successfully make requests an API key is required using the x-api-key header. In development these keys are seeded at startup and are as follows: 

* 9a3192fc-cb2d-487a-a377-8c6c8b60e007
* 1215fbfb-7613-4810-bd1a-2e529c3484e9

If you submit a payment using one API key, you can only view it again using the same one.

# System Design
The solution has three main projects (API, Service, and Data), with a Core project being shared between them. 
The API project is a Web API project, and contains:

 - The Payment Controller
 - The authentication handling
 - Exception handling middleware
 - Models for requests/responses to the API

 It is also the entry point to the application and as such contains all the app configuration/dependency injection.
 
 The Service project is a class library which contains applications business logic including:

 - Validators
 - Extensions
 - Acquiring Bank HTTP client
 -  The payment service

The Data project is also a class library which is responsible for seeding the database, and providing other projects within the solution access to the data. It contains: 
 - Entities used by EntityFramework
 - Repositories
 - EntityFramework migrations
 - A DB context factory to get around entity framework not being in the startup project

There is also a Core  project which is once again a class library and contains shared models, enums, config settings, and a DateTimeProvider to aid in unit testing,

Were this to be used in a production environment, changing from the acquiring bank mock to an actual API should just be a simple case of changing the URL in the appsettings.json file (although there would likely be additional complexity with security). 

This application could also be architected in a more Microserve-esque way, by having an application for the API, and an application for the service/data. This would have allowed the services to be scaled independently of the API, however since it was my first time using Docker I tried to keep things simple. Technically, having the SQL server within the docker containers is also not likely to happen in a production environment, however it should be possible to change the connection string to point at a production instance. 

# Improvements

 - As mentioned above, removing segmenting the solution into separate API/Service solutions would likely be an improvement
 - In a real development environment this project would be scoped out, broken down into individual tickets with clear ACs, and work would be done on actual branches with CR/QA process rather than just slammed into master.
 - The validation is weak, and could be improved massively. There are also sweeping assumptions made around card details, etc. This could likely be improved by calling an actual card validation API.
 - The API key implementation was done following a tutorial and doesn't offer much more than basic authorization/authentication. It is also not very extensible so if we wanted to use it for more than just merchants there would be some pain.
 - My knowledge on PCI compliance isn't great, but I know there are improvements that can be made around how card details are handled/stored, such as tokenization [https://www.pcisecuritystandards.org/documents/Tokenization_Guidelines_Info_Supplement.pdf](https://www.pcisecuritystandards.org/documents/Tokenization_Guidelines_Info_Supplement.pdf)
 - The http client for connecting to the bank mock could potentially have a retry mechanism added. I am wary of adding retry mechanisms, especially when dealing with third parties/anything to do with payments. 
 - Further testing is never a bad thing. 