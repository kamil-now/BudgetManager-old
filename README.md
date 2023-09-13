## WORK IN PROGRESS
---
### Run API on localhost

You need to make sure that the [Docker Desktop](https://docs.docker.com/get-docker/) application is installed and running on your system. 

The Docker Desktop installer usually sets up the Docker daemon as a Windows service, which starts automatically when you log in to your system. 

MongoDB should be configured automatically when you run the API.

For `#DEBUG` mode the authentication is mocked, user id defaults to "mock_user_id" and is defined in MockJwtAuthenticationHandler.

Go to /source/BudgetManager.Api and run:

```dotnet run```

If it doesn't redirect you automatically go to http://localhost:3000/swagger/index.html.

### Run UI on localhost

Go to /source/BudgetManager.UI and run:

```npm i``` (when running for the first time)

then

```npm start```

Go to http://localhost:8200

---

### Run tests

Go to /source/BudgetManager.Application.Tests and run:

```dotnet test -l "console;verbosity=detailed"```

### Debug tests in VS Code

Ctrl+Shift+P -> Tasks:Run Task -> test

This will start process and should ouput 'Process Id: {your process id}' in the terminal

Ctrl+Shift+D -> .NET Core Attach -> find your process by id and select it

### Run in Docker

```docker build -t budget-manager-app .```
```docker run -p 8080:80 --name custom-container-name budget-manager-app```