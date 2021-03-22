# Product.API
---
### Product.API is a .Net Core Web API application that allows to create as well as get/update/delete simple products with their options.

##### *Prerequisites*
- [x] Visual Studio 2019 + .Netcore 3.1
- [x] Nuget packages: Dapper, Microsoft.Data.Sqlite, Newtonsoft.Json, NLog.Web.AspNetCore, Swashbuckle.AspNetCore, Moq, NUnit

##### *Assumptions*
1. There is no authentication/authorization for create, update and delete products/product options actions. However when deploying to production this must be implemented along with SSL to protect our data.
2. Database connection string, secrets, tokens etc should be stored in more secure places such as AWS Secret Manager or Parameters Store etc instead in appsettings.json file. That way our credentials won't expose to public via checkins.  
3. Data will be persisted into local database file called *products.db* under *App_Data* folder. In production, it should be replaced by a more powerful RDBMS instance.

##### *Entry point*
`Program.cs`

**How to run the app**
```
1. Double click on the solution file Product.API.sln, this should bring up Visual Studio 2019
2. Hit F5 to run, this should bring up the browser with this url loaded https://localhost:5001/swagger/index.html. This is the Swagger API interface that helps us execute REST API calls easily with minimum effort.
3. Choose an API for example GET /api/Product/v1/products, hit the Try it out button then hit the Execute button then check the result from the underneath pane. 
```

*Project description & requirements*
- Provide endpoints for basic *CRUD* operations for products and their options.
- Persist products and options into local database.
- Prevent LOST UPDATE issue when there are multiple API requests trying to update the same product.
- Unit tests to make sure business logic implementations are as expected.
- Web API documentation - as this project is pretty simple therefore the Swagger API tool should do the job. Most of APIs are self explanatory with different HTTP verbs and RESTful urls otherwise please check the comments in the codebase.

*Design Patterns & Implementations*
- Applied *Abstract Factory pattern* to create database connection for the application therefore any service that relies on this for example the IRepository does not need to know how the database connection is constructed.
- Applied *Repository pattern* for CRUD operations for any object types.
- Applied *optimistic locking mechanism* by using *ETag* and *If-Match* in request and response headers to prevent the LOST UPDATE issue. I chose this approach because it is lightweight, less resource intensive & easy to implement over the use of database locking mechanism. Reference: https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/perform-conditional-operations-using-web-api. To be honest I haven't looked deeper and measured the *optimistic locking* at database level so I maybe wrong with my statement above, so let me know ;).

*Questions & Answers*
- Q: So how do I do an update to an existing product then?
- A: First, grab the hashed string from the response header named *ETag* when you get the product details by hitting this endpoint *GET /api/Product/v1/products/{productId}*. Second, pass the hashed string into the request header named *If-Match* when you update the product by hitting this endpoint *PUT /api/Product/v1/products*. The underlying logic will tell you whether your update request is accepted if there is no changes or rejected if there are changes since you have called the *GET /api/Product/v1/products/{productId}* endpoint. In this case you need to repeat the first step and second step in order to update the product successfully.


**Happy coding**
