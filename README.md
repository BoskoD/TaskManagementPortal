# TaskManagementPortal

## Implementing a task storage by projects. 
“Task” – is an instance which contains at least 3 fields listed below:
1.	Id
2.	Task name
3.	Task description
### Solution should provide an ability to easily add new fields in Task entity. 
Each task should be a part of only one project. Project – is an instance which contains name, id and code (and also keep Tasks entities).
You need to deploy your service in Microsoft Azure and use tables (Azure Table Storage) for storing your projects and tasks (you can use free subscription). To access to your service you need to implement Rest API.

# Requirements:
#### Need to store your solution in github/bitbucket and provide an access to your mentor
#### Need to split your project by parts, evaluate each part (in hours). Commit this plan in repository (as separate file)
#### After all you can start your implementation based on plan above. Please track your time. Your commit history is important


# Parts
- Add connection to Azure Storage Table
- Add models and entities
- Add repositories and interface
- Configure controllers
- Make everything async
- Add DTO objects
- Add swagger
- Add basic health checks
- Add logging
- Implement isComplete property
- Add swagger description
- Add authentication [JWT]
- Add CORS
- Deploy to Azure

# TODO
- Angular client
- Refactor Update
- Add mapper


