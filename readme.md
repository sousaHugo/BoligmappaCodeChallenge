
# Boligmappa Code Chagllente - Hugo Sousa

## 1. Requirements
Consider the following dummy API [https://dummyjson.com (https://dummyjson.com/). This API has Users (https://dummyjson.com/users) and User Posts (https://dummyjson.com/posts) and User Todos (https://dummyjson.com/todos) and I would like to have my two different applications to access this external API and perform some queries. Write Unit Tests for all the necessary scenarios.

 - Please explain how you would go and develop this solution. Try to use SOLID and DRY principles.
 - 1st  Application (Console Application) to Create a PostgreSQL table to store some user data (UserId, number of user posts, number of user todos).
	 - This data comes from filtering the API for Posts with at least one reaction and have history tag.
	 - Expect to use Entity framework for this.
 - Perform some queries on the 1st Application:
	 - Find the Users with more than 2 posts and show their todo's.
	 - Finds Users that uses “mastercard” as their cardtype and retrieve their posts.
 - 2nd  Appplication (Console Application) to Create a PostgreSQL to store Posts (PostId, UserName, hasFrenchTag, hasFictionTag, hasMorethanTwoReactions).
	 - Expect to use Entity framework for this.
 - How would be your approach in deploying this solution to AWS.

## 2. Solution
For the development of this solution I decided to divide it into two distinct parts.

 - One of them, and since the API provided is dummy, was to develop a dummy Api in order to be able to carry out development tests (Api Folder).
 - The other part is divided into the two Console Applications that were requested (Folders FirstApplication, SecondApplication and Shared).
### 2.1. Api Folder
In this folder I decided to create 3 different projects (*BCCP.DummyApi*, *BCCP.DummyGrpc* and *BCCP.DummyAggregatorApi*).
 - BCCP.DummyApi:
	 - This project represents the dummy API that was referred to in the Code Challenge statement. As only 3 endpoints were mentioned in the statement and not knowing what operations this Api would have, I decided to create some specific operations, including GetAll (gets all records from Posts, Todos and Users.)
 - BCCP.DummyGrpc:
	 - During our conversation at Inscale, it was mentioned that Boligmappa used gRPC in their projects. Since recently during my studies I came across this technology I decided to also implement a gRPC dummy API.
 - BCCP.DummyAggregatorApi:
	 - Finally, and as I also came across this possibility in my most recent studies, I also decided to implement an Api following the concept of Api Gateway Aggregator. This API can return to the client an object that contains all the necessary information for the client. Since the Aggregator information is obtained from the Dummy Api gRPC.
### 2.1. FirstApplication and SecondApplication Folders
In these two folders we can find the implementation of the two Console Applications requested in the Code Challenge. For both applications I decided to use the same architecture. Having decided to use the Clean Architecture structure in order to be able to implement all the SOLID and DRY concepts.

In addition to this structure, in both projects I applied concepts such as Repository Pattern (abstract data access layer) and MediatR Pattern (mainly reduce dependency between objects).
#### 2.1.1. Project Structure

 - **Project AppConsole:** This project is responsible for the presentation layer as well as for building all the dependencies that will be injected during the execution of the program (ProgramHostBuilder).
 - **Project Application:** This project is responsible for all Core functionalities of the application, such as all Features (implementation), Contracts, Custom Exceptions, Data Transfer Objects, etc.
 - **Project Domain:** This project just presents all the domains (entities that represent the database tables) of the application as well as models that represent the objects coming from the gRPC calls.
 - **Project Infrastructure:** This project is responsible for the entire infrastructure of the application, that is, the definition of the DB Context, the data access layer (Repository), invocation of the different Api (Web Api and gRPC) and for the Proto Clients (gRPC Client classes).
 - **Project Tests:** This project is responsible for implementing the tests for all Features of the project.
### 2.2. Shared Folder
In this project we can find all objects used for abstraction and reuse, that is, generic objects for Dependency Injection (DbExtension), Repository operations (BaseRepository), generic object for calls to the Api (BaseService), etc.
### 2.3. Docker Compose
It was also not requested in the Code Challenge but I thought it might be interesting to implement in this solution. In this case, the implementation of Docker Orchestration will make all API's as well as the database server and its administration panel launched and can be used by ConsoleApps.
## 3. Use Cases
### 3.1. First Application - Features
 - **GetAllUserInformation:** This feature retrieves all Users stored in the database.
 - **GetFromDummyApi:** As I don't know which methods are provided by the Dummy Api, this functionality assumes that the existing operations only return all the records (Users, Todos, Posts). For its implementation, follow these steps:
	 - Get all Posts through the Dummy Api and validate which of them have at least one Reaction and contain the History Tag.
	 - If there is any Post in these conditions, get all Users and all Todos.
	 - Construct the object to be saved to be validated.
	 - Validate the object.
	 - If the object already exists, just an Update is performed otherwise a Create is performed.
 - **GetFromDummyApiAggregator:** As I mentioned before and although it was not requested, I added this functionality. If the APIs are ours, we could create an Aggregate so that it would return all the information we need in a single request. In this way, this functionality was implemented following these steps:
	 - Get the information from the Aggregator.
	 - Convert that information to the type of object that will be later validated and saved.
	 - Validate the object.
	 - If the object already exists, just an Update is performed otherwise a Create is performed.
 - **GetFromDummyApiExtra:** As I mentioned earlier and as I don't know which endpoints are provided by the Api, in this feature I decided to assume that the Api has a method that returns Posts based on a Tag. In this way, the representative object of the User is constructed, which will then be validated and subsequently updated (if it already exists) or created (if it does not yet exist).
 - **GetFromDummyApiGrpc:** As I mentioned earlier and as in our previous conversation we talked about the use of gRPC, I decided to also implement a Feature similar to GetFromDummyApi but in which the calls are made to the gRPC server.
 - **GetPostsUsersMasterCard:** This functionality obtains all the Users that use MasterCard and that are saved in the DB. For each of these Users, a call is made to the Dummy Api, returning their Posts.
 - **GetTodosUsersMoreTwoPosts:** This functionality retrieves all Users that have more than two Posts and that are stored in the DB. For each of these Users, a call is made to the Dummy Api, returning their Todos. For each registration, a request is also made to the Users API and if the User is found from Todos.

### 3.2. Second Application - Features
 - **GetAllPostInfo:** This feature retrieves all information related to the Posts stored in the DB.
 - **GetPostsFromDummyApi:** This feature gets all the information related to the Posts through an HTTP call to the Dummy API. Then the application checks whether each of these Posts is valid to be saved (the objects are validated using the FluentValidation library). If the object is valid for saving, a new validation is performed. If the object already exists in the database, it is updated. Otherwise, it will be created.
 - **GetPostsFromDummyApiGRpc:** This feature retrieves all information related to the Posts through a gRPC call.
## 4. Technology / Frameworks
 - AutoMapper
 - FluentValidation
 - .NET 6
 - Docker
 - Docker Desktop
 - MediatR
 - gRPC
 - Serilog
 - xUnit
 - Moq
 - Shouldly
 - EntityFrameworkCore
 - PostgreSQL
 - Swashbuckle
## 5. Run / Debug
To run the application without using Docker Compose, just run it. The URLs that are parameterized in the different projects (appsettings.json) are defined for when the applications are launched locally.

To use Docker Compose, all Console Apps settings (appsettings.json ) must be updated to the correct endpoints.

## 6. AWS Deploy

## 7. Conclusion
It is clear that this project is just an exercise and that it could be improved in some aspects. However, I think it meets all the requirements that were asked in the statement. And beyond that, I think it has even more features that demonstrate some knowledge beyond what is required, as is the case for example with the gRPC Server or the Gateway Aggregator.
