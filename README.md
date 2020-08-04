# Installation

The payment gateway, SQL server data store, and acquiring bank mock have all been containerised using docker. Running this solution it *should* just be as simple as cloning the project and running the following from the root directory (please ensure ports 1433, 5000, and 6000 are all clear)
```
docker-compose up
```
Once all three containers are running you can make requests to the API from your favorite REST client. To ensure that both APIs are up and running you can make a call to http://localhost:5000/health and http://localhost:6000/health. The API contract can be found by going to http://localhost:5000/swagger

# Testing

To successfully make requests an API key is required using the x-api-key header. In development these keys are seeded at startup and are as follows: 

* 9a3192fc-cb2d-487a-a377-8c6c8b60e007
* 1215fbfb-7613-4810-bd1a-2e529c3484e9

Currently there is no method for adding a new API Key unless you wish to manually insert it into the DB tables.

## Payment Submission Endpoint
The mock bank uses different CVV codes to determine which status to return, currently the following three bank responses are implemented:

 - **BankValidationError** - CVV Code : 666
 - **SubmissionError** - CVV Code : 700
 - **Submitted** - Any other CVV code

To test the submission process, a **POST** request should be sent to http://localhost:5000/api/v1/payments with the following body: 
```
{
	"Amount": 500.00,
	"CurrencyIsoAlpha3": "GBP",
	"CardNumber": "1111 2222 3333 4444",
	"Cvv": "600",
	"CardholderName": "Testy McTester"
}
```
The response will always be 200 with the following body:
```
{
	"paymentId": "c00d9b04-6c6f-4591-3ef5-08d838650390",
	"paymentStatus": "Submitted",
	"validationErrors": null
}
```
The paymentStatus/ValidationErrors will be populated based on the CVV and the validity of the data submitted. To test the retrieving payment details functionality please note down the paymentId.

## Payment Retrieval Endpoint
To test the retrieval process, a **GET** request should be sent to http://localhost:5000/api/v1/payments/{paymentId} where {paymentId} is an ID returned from the submission endpoint. It is important that the **x-api-key** header is the same as the one used for the submission process, otherwise you will not be able to get the payment details.

If there is not a valid payment, or the payment is not linked to the same merchant that is submitting the request, a 204 response will be returned. If a payment is found then the response will be 200 with the following body:
```
{
	"amount": 500.00,
	"currencyIsoAlpha3": "GBP",
	"maskedCardNumber": "1111********4444",
	"cardholderName": "Testy McTester",
	"latestPaymentStatus": "Submitted"
}
```

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

This application could also be architected in a more Microservice-esque way, by having an application for the API, and an application for the service/data. This would have allowed the services to be scaled independently of the API, however since it was my first time using Docker I tried to keep things simple. Technically, having the SQL server within the docker containers is also not likely to happen in a production environment, however it should be possible to change the connection string to point at a production instance. 
# Extras implemented

### Application logging
Logging was done using Serilog which simply logs to the console. In a production environment this would be hooked up to  a sink to write the logs to an external log aggregation service, such as Google Stackdriver. 
As part of this a global exception handling middleware was also implemented. This will handle any uncaught exceptions during the lifetime of a request. Any exceptions caught are logged, and a generic error response is returned to the API caller.

### Containerization 
I have not used containers before professionally, so this was a good learning opportunity for me. I opted to use docker to containerize the solution as it seems to be the most widely used, and has decent documentation behind it. The SQL Server docker container also uses volumes to persist data.

### Authentication
Authentication/Authorization is done using API keys following a tutorial online. Once again this is not something that I had previously implemented, so it was a fun learning experience.
### API Client
Swagger was added as the API client.
### Data Storage
For the purpose of this challenge a containerized SQL Server instance was used. This persists between subsequent app launches. 

# Improvements

 - As mentioned above, separating the solution into separate API/Service solutions would likely be an improvement
 - In a real development environment this project would be scoped out, broken down into individual tickets with clear ACs, and work would be done on actual branches with CR/QA process rather than just slammed into master.
 - The validation is weak, and could be improved massively. The validation used here is more for proof of concept than something that is robust enough to actually be used. This could likely be improved by calling an actual card validation API.
 - The API key implementation was done following a tutorial and doesn't offer much more than basic authorization/authentication. It is also not very extensible so if we wanted to use it for more than just merchants there would be some pain. Further investigation into authentication/authorization best practices should be undertaken to improve this aspect of the solution.
 - My knowledge on PCI compliance isn't great, but I know there are improvements that can be made around how card details are handled/stored, such as tokenization: [https://www.pcisecuritystandards.org/documents/Tokenization_Guidelines_Info_Supplement.pdf](https://www.pcisecuritystandards.org/documents/Tokenization_Guidelines_Info_Supplement.pdf)
 The card details should also not be stored in plain text, and should ideally be encrypted, however more investigation would be required to understand the best practices around storing card details.
 - The http client for connecting to the bank mock could potentially have a retry mechanism added. I am wary of adding retry mechanisms, especially when dealing with third parties/anything to do with payments as there is a possibility that the payment is saved but another part of the third party API fails before returning.
 - Further testing is never a bad thing, this solution is lacking integration testing due to time constraints. I have not done performance testing before so that is something that I should learn more about.
 - The remaining extra mile bonus points.