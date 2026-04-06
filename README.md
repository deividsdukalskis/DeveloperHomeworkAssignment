# Homework assignment
## Setup
### Prerequisites
- Docker Desktop installed https://www.docker.com/
- .NET 9 Build Tools with ASP.NET packages
### Steps
- Setup docker to host PostgreSQL database. Run this inside terminal at the solution root directory

    ```
    docker compose -f docker-compose-postgres.yml up -d
    ```

- EF Core migration scripts are already in project but if you need to create new migrations then run this inside terminal at the API project root directory.

    ```
    dotnet ef migrations add migration_name -o DataAccess/Migrations
    ```
    
- Migrations are applied automatically on every API project startup for development purposes
## Project structure
Solution consists of one ASP.NET Web API project that encapsulates DTOs, DB access and logic.

Due to the small nature of the project this is the best approach. If for some reason some project layering would be beneficial then moving files around would not be a problem.

### Minimal APIs

Minimal APIs are used instead of traditional controllers because of their friendly relationship with feature slicing.

Although requirements are CRUD in nature like `POST "api/users"` or `GET "api/users/{id}"` and in a sense would encourage to use controllers, I would reform `POST "api/users"` to something like `POST "api/create-user-and-profile"` in a more business oriented enterprise application. This removes direct DTO to DB entity thinking and focuses more on business flow.

That does not mean that CRUD is all bad. For example - one page where all the fields are saved without much logic. All that you want is some CRUD operations.

### Feature slicing (Vertical slice)
Very good organizational choice if you want maximum isolation between features. Very good against "one change breaks it all".

That does not mean that everything has to be inside one file. Could be inside one folder or some other hybrid approach. Rule is that logic is contained to one feature.

One file in this case because of tiny logic behind.

Also contract (request/response) owns all the fields and does not share nothing between them even if it seems identical. Reason is that they might change separately and API client wants the data they ask and not some weird empty fields coming from another place.

Regards of contract naming just Response, Request, there was a problem with Swagger not recognizing C# class hierarchy but since Swagger is no longer used so it works fine now.

### Functional programming
Very good for branch flow and removes lots of boilerplate code.

Downsides - Harder to debug especially when no expression body is used.
Maybe downside - If you are used to OOP and only think in OOP. This might seem abusive if everything is static method. But C# is meant to be used as OOP and Functional together wherever they suit the best.

### Custom error handling
Lots of projects use exceptions for error handling and there is nothing wrong with that to some level.

But custom error handling adds the benefits for chaining, custom workflow configuration and predictable results, whereas exceptions are just throwing you out of the window.

I reserve exceptions for something unpredictable, hence the name - exception.

### Global exception handling
This is a must for every project. Decreases security vulnerabilities as exposed stack trace with internal system structure. Neat way to organize how exceptions get handled and show just the place where exception details are stored only for developer eyes.

### Message broker stub
Removed `Task` because no `async` methods are ran inside the method. Also plugged in some quick mapping possibility from other type to message contract type so no additional hard typed mapper needed.

## Example requests
#### POST
- Valid User
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"john.doe_1","email":"john.doe@example.com","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1995-06-15T00:00:00Z"}}'
```
- Valid Today birthday
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"today.valid","email":"today.valid@example.com","profileInformation":{"firstName":"Today","lastName":"Valid","dateOfBirth":"2026-04-06T00:00:00Z"}}'
```
- Invalid Username exists
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"john.doe_1","email":"john.dsoe@example.com","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1995-06-15T00:00:00Z"}}'
```
- Invalid Email exists
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"john.doe_1s","email":"john.doe@example.com","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1995-06-15T00:00:00Z"}}'
```
- Invalid Username empty
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"","email":"nouser@example.com","profileInformation":{"firstName":"No","lastName":"User","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Username number
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"1johnsmith","email":"johnsmith@example.com","profileInformation":{"firstName":"John","lastName":"Smith","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Username double
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"john..doe","email":"john.doe@example.com","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Username enddot
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"johndoe.","email":"johndoe@example.com","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Email
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"johnvalid","email":"not-an-email","profileInformation":{"firstName":"John","lastName":"Doe","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Profile null
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"johnvalid","email":"john.valid@example.com","profileInformation":null}'
```
- Invalid First name
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"johnvalid","email":"john.valid@example.com","profileInformation":{"firstName":"","lastName":"Doe","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Last name
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"johnvalid","email":"john.valid@example.com","profileInformation":{"firstName":"John","lastName":"Doe123","dateOfBirth":"1990-01-01T00:00:00Z"}}'
```
- Invalid Future birthday
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"future.person","email":"future.person@example.com","profileInformation":{"firstName":"Future","lastName":"Person","dateOfBirth":"2099-01-01T00:00:00Z"}}'
```
- Invalid Old birthday
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"historic.user","email":"historic.user@example.com","profileInformation":{"firstName":"Historic","lastName":"User","dateOfBirth":"1800-01-01T00:00:00Z"}}'
```
- Invalid Multiple
```
curl -X POST "http://localhost:5160/api/users" \
  -H "Content-Type: application/json" \
  -d '{"username":"1bad..name","email":"bad-email","profileInformation":{"firstName":"","lastName":"Doe123","dateOfBirth":"2099-01-01T00:00:00Z"}}'
```
#### GET
- Get User (replace {id} with actual value)
```
curl --location --request GET 'http://localhost:5160/api/users/{id}'
```