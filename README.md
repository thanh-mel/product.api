# Product.API
---
### Product.API is a .Net Core Web API application that allows to create as well as update/delete simple products with their options.

##### *Prerequisites*
- [x] Visual Studio 2019 + .Netcore 3.1
- [x] Nuget packages: Dapper, Microsoft.Data.Sqlite, Newtonsoft.Json, NLog.Web.AspNetCore, Swashbuckle.AspNetCore, Moq, NUnit

##### *Assumptions*
1. There is no authentication/authorization for create, update and delete products/product options actions. However when deploying to production this must be implemented to protect our data.
2. Database connection string, secrets, tokens etc should be stored in more secure places such as AWS Secret Manager or Parameters Store etc instead in appsettings.json file. That way our credentials won't expose to public via checkins.  

##### *Entry point*
`Program.cs`

**How to run the app**
```
1. Double click on the solution file Product.API.sln, this should bring up Visual Studio 2019
2. Hit F5 to run, this should bring up the browser with this url loaded https://localhost:5001/swagger/index.html. This is the Swagger API interface that helps us execute REST API calls easily with less effort.
3. Choose an API for example GET /api/Product/v1/products, hit the Try it out button then hit the Execute button then check the result from the underneath pane. 
```

*Notes*
- In some cases csv files maybe big that could make the application not responsive, therefore when reading large csv data the application uses async await mechanism to load data as stream. Tested with more than 3+ million records in a catalog csv file and CPU usage was just around 50%.
