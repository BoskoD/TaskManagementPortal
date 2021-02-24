# TaskManagementPortal

## Implementing a task storage by projects. 
“Task” – is an instance which contains at least 3 fields listed below:
1.	Id
2.	Task name
3.	Task description

### Solution should provide an ability to easily add new fields in Task entity. 
Each task should be a part of only one project. Project – is an instance which contains name, id and code (and also keep Tasks entities).
You need to deploy your service in Microsoft Azure and use tables (Azure Table Storage) for storing your projects and tasks (you can use free subscription). 
To access to your service you need to implement Rest API.


# Timeline
- Add connection to Azure Storage Table
- Add models and entities
- Add repositories and interface
- Configure controllers
- Make everything async
- Add DTO objects
- Add swagger
- Add logging
- Add isComplete property on Task
- Add swagger description
- Add authentication [JSON WEB TOKEN]
- Add CORS
- Add Angular client
- Update .NET 5
- Deploy to Azure


# TODO
- Material Design
- Add auto mapper
- Add batch import/export


