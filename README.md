# Product.API
---
### Product.API is a .Net Core Web API application that allows to create as well as update/delete simple products with their options.

##### *Prerequisites*
- [x] Visual Studio 2019 + .Netcore 3.1
- [x] CsvHelper Nuget package
- [x] Nlog.Web.AspNetCore Nuget package

##### *Assumptions*
1. The application tries to merge product catalogs from 2 different sources (in this case source is a company) only.
2. CSV file naming convention:
    - catalog<company-name>.csv: products for the company
    - suppliers<company-name>.csv: list of suppliers for the company
    - barcodes<company-name>.csv: Product barcodes provided by supplier for the company

##### *Entry point*
`CatalogMergeHostedService.cs`

**How to run the app**
```
1. Double click on the solution file CodeChallenge.sln, this should bring up Visual Studio 2019
2. Hit F5 to run
3. Check result in /Data/Output folder
```

*Notes*
- In some cases csv files maybe big that could make the application not responsive, therefore when reading large csv data the application uses async await mechanism to load data as stream. Tested with more than 3+ million records in a catalog csv file and CPU usage was just around 50%.
