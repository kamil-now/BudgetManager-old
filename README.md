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

---

### Run tests

Go to /source/BudgetManager.Application.Tests and run:

```dotnet test -l "console;verbosity=detailed"```

