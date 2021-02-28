# TaskManagementPortal

“Task” – is an instance which contains at least 3 fields listed below:
1.	Id
2.	Task name
3.	Task description

“Project” – is an instance which contains at least 3 fields listed below:
1. Id
2. Project name
3. Code base

## Solution 
We should have the ability to easily add new fields in Task entity. 
Each task should be a part of only one project. 
Project – is an instance which contains name, id and code (and also keep Tasks entities).
Service needs to be deployed in Microsoft Azure and use tables (Azure Table Storage) for storing projects and tasks. 
To access to our service we need to implement Rest API.

## Technologies

- .NET Core (5.0)
- Azure Table Storage
- Angular

## API Documentation - Swagger
API documentation V1: https://taskportalapi20210227224026.azurewebsites.net/index.html

## TODO

- [x] Async/Await
- [x] REST
- [x] KISS
- [x] SOLID
- [x] JWT
- [x] Repository & Generic Repository
- [x] API Specification, API Definition (Swagger)
- [x] Postman
- [x] Middleware
- [x] Authentication
- [x] Authorization
- [x] Inversion of Control / Dependency injection
- [x] CORS
- [x] Error Handling, Global Exception
- [x] HealthCheck
- [x] Soft Delete
- [x] Scoped over Transient
- [x] Logging
- [x] StyleCop
- [x] API Versioning with Swagger
- [x] Caching
- [x] Kestrel
- [x] AzureStorage
- [x] NLog
- [x] NET5
- [x] Angular
- [x] PSScripts
- [ ] Mapping (AutoMapper)
- [ ] Docker
- [ ] File storage: Upload/Download
- [ ] Kubernetes / SF
- [ ] BulkInsert, BulkUpdate
- [ ] File manager
- [ ] Material design
- [ ] StorageTable enum conversion
- [ ] Unit Test
- [ ] Moq
 
 
## Documentation
- https://docs.microsoft.com/en-us/dotnet/api/overview/azure/storage
- https://docs.microsoft.com/en-us/azure/storage/tables/table-storage-how-to-use-powershell
- https://nlog-project.org/
- https://angular.io/


