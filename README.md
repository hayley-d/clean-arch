# Codehesion C# minimal template

## This is a dotnet template and should be installed first.

### Step 1: Install the template 
Pull this repo, but don't restore or build as this will add bloat the usage of the template on your system

Run this command to install the template to dotnet.
```bash
dotnet new install <PATH_TO_THIS_REPO>
```
The template is now installed as a custom dotnet template called `codehesion-minimal`.

### Step 2: Create a new project
Open your terminal and navigate to your GIT Repos folder. You can create you project by 
running this command:

>**DO NOT USE SPACES IN *<NAME_OF_YOU_PROJECT>***.

```bash
dotnet new codehesion-minimal --name <NAME_OF_YOU_PROJECT>
```
The following will be created:

`Repos/NAME_OF_YOU_PROJECT/src`

you can now navigate to your project and continue there.

### Step 3: Update the README 
Assuming that this is your project and not the template repo. 
Remove everything above this line.

-------------------------------------------
# minimal Web API

use docker compose up for spinning up persistence containers
```bash
docker compose -p minimal -f docker-compose.yml up -d
```

run this to remove all `.DS_Store` files in the project directory
```bash
find . -name ".DS_Store" -type f -delete
```

You will see the following projects created in the src folder
- Application
- Contracts
- Domain
- Infrastructure
- Presentation
- Shared

## Contracts Project
Use this project to store requests and responses (DTOs),
it should only contain the class. ***Do not implement the mappings here***.
Mappings should be implemented in the `Application layer` as extension methods.
See `UserExtensions.cs` for an example of how this should be done.

## Shared Project
You will likely not touch this project, but know that it contains Error abstractions and the Result implementation.

## Clean Architecture Projects
The rest of the projects: `Presentation, Application, Domain, Infrastructure` 
are all standard Clean Architecture concepts.
See [Clean Architecture](https://www.milanjovanovic.tech/blog/clean-architecture-and-the-benefits-of-structured-software-design) for more details.

## What's new
1. Minimal API's as the new standard.
2. `ApiEndpoints.cs` should be used to define API routes.
3. `ISender` is used instead of `IMediator`.
4. `sender.Send()` now returns `Task<Result<TResponse>>`.
5. Mediator validation pipeline does not throw anymore, instead, it returns `Result<TResponse>`
containing a ValidationError. This can be mapped to a `ValidationProblem`. See `AbstractErrorExtensions.cs`
6. Custom exceptions have been replaced with Errors, see `NotFoundError.cs`
7. Errors seamlessly plug into the `Result` object.
8. Request Handlers should now return `Result` or `Result<T>`.
9. Strongly typed entity IDs as the new standard, see `UserId.cs`.
10. Strongly typed entity IDs must implement `IParsable<T>` in order to use them as route parameters. 
See `GetUserEndpoint.cs`.
11. OpenTelemetry Activities have been added. `LoggingPipelineBehavior.cs line 23` show how an activity should be used.
12. Extension methods have been created for `Blob` to convert from `IFormFile` to `blob`,
and converting `Blob` to `FileContentHttpResult`