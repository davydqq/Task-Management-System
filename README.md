## Task-Management-System
This is the README file for the application Task-Management-System. The application is built using .NET Core and utilizes RabbitMQ for messaging. This guide will provide you with instructions on how to run the application.
## Prerequisites
Before you proceed with the installation, please ensure that you have the following prerequisites installed on your system:

Docker or RabbitMQ
.NET Core SDK 6

### Installation
Please follow the steps below to run the application locally:

1. Clone the repository to your local machine:
**git clone git@github.com:davydqq/Task-Management-System.git**
2.  Run the command if the Docker is installed, or make sure RabbitMQ is installed.
**docker run -d --name my-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:management**
3. Change the connection strings to Database or RabbitMQ if you have your own settings.
**TaskManagementSystem\TaskManagementSystem\appsettings.Development.json**
4. Database Initializing
Using Package Manager Console run **update-database** command.
Using EF Core command-line tools: **dotnet ef database update** in TaskManagementSystem\TaskManagementSystem folder
5. Run project usigng visual studio or use CLI dotnet run Program.cs in TaskManagementSystem\TaskManagementSystem folder
6. After running use swagger to interact with system 
https://localhost:7123/swagger/index.html
7. Use RabbitMQ Management to see messages, queues etc.
http://localhost:15672/#/queues