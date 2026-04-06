namespace DeveloperHomeworkAssignment.API.Features
{
    using DeveloperHomeworkAssignment.API.DataAccess;
    using DeveloperHomeworkAssignment.API.DataAccess.Entities;
    using DeveloperHomeworkAssignment.API.Messaging.User;
    using DeveloperHomeworkAssignment.API.Utilities;
    using Microsoft.AspNetCore.Mvc;

    public static class CreateNewUserAndProfile
    {
        public static readonly string Uri = "/users";

        public record Request(
            string Username,
            string Email,
            Request.Profile ProfileInformation)
        {
            public record Profile(
                string FirstName,
                string LastName,
                DateTime DateOfBirth);
        }

        public record Response(
            Guid UserId,
            string Username,
            string Email,
            Response.Profile ProfileInformation)
        {
            public record Profile(
                string FirstName,
                string LastName,
                DateTime DateOfBirth);
        }

        public static IResult Endpoint([FromBody] Request request, DHADbContext dbContext, IUserMessagePublisher messagePublisher)
        {
            return request.Validate()
                .Evaluate(onSuccess: () =>
                {
                    Users user = request.MapRequestToEntity();
                    if (dbContext.Users.Any(x => x.Email == user.Email))
                    {
                        return Results.Conflict(string.Format(Errors.EmailAlreadyExists, user.Email));
                    }

                    if (dbContext.Users.Any(x => x.Username == user.Username))
                    {
                        return Results.Conflict(string.Format(Errors.UsernameAlreadyExists, user.Username));
                    }

                    var result = dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    messagePublisher.PublishUserCreated(user, map: (user) =>
                    {
                        return new UserCreatedMessage(user.Id, user.Username, user.Email);
                    });

                    Response response = result.Entity.MapEntityToResponse();
                    return Results.Created($"/api/users/{response.UserId}", response);
                },
                onFailure: (errors) => errors.MapToHttpResult());
        }

        private static Users MapRequestToEntity(this Request request)
        {
            return new Users()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
                Profile = new Profiles()
                {
                    FirstName = request.ProfileInformation.FirstName,
                    LastName = request.ProfileInformation.LastName,
                    DateOfBirth = request.ProfileInformation.DateOfBirth,
                },
            };
        }

        private static Response MapEntityToResponse(this Users entity)
        {
            return new Response(
                entity.Id,
                entity.Username,
                entity.Email,
                new Response.Profile(
                    entity.Profile.FirstName,
                    entity.Profile.LastName,
                    entity.Profile.DateOfBirth));
        }

        private static Result<ErrorCollection<ValidationError>> Validate(this Request request)
        {
            var firstAttempt = request.Username
                    .ShouldBe(x => !string.IsNullOrEmpty(x), nameof(request.Username), string.Format(Errors.FieldCannotBeEmpty, nameof(request.Username)))
                    .ThenShouldBe(x => x.Length <= 100, nameof(request.Username), string.Format(Errors.FieldValueTooLong, nameof(request.Username)))
                    .ThenShouldBe(x => x.IsValidUsername(), nameof(request.Username), Errors.InvalidUsername)
                .And(() => request.Email
                    .ShouldBe(x => !string.IsNullOrEmpty(x), nameof(request.Email), string.Format(Errors.FieldCannotBeEmpty, nameof(request.Email)))
                    .ThenShouldBe(x => x.Length <= 254, nameof(request.Email), string.Format(Errors.FieldValueTooLong, nameof(request.Email)))
                    .ThenShouldBe(x => x.IsValidEmail(), nameof(request.Email), string.Format(Errors.InvalidEmail, request.Email)))
                .And(() => request.ProfileInformation
                    .ShouldBe(x => x != null, nameof(request.ProfileInformation), string.Format(Errors.FieldCannotBeEmpty, nameof(request.ProfileInformation))));

            if (!firstAttempt.IsSuccess)
            {
                return firstAttempt;
            }

            var secondAttempt = request.ProfileInformation.FirstName
                    .ShouldBe(x => !string.IsNullOrEmpty(x), nameof(request.ProfileInformation.FirstName), string.Format(Errors.FieldCannotBeEmpty, nameof(request.ProfileInformation.FirstName)))
                    .ThenShouldBe(x => x.IsValidName(), nameof(request.ProfileInformation.FirstName), string.Format(Errors.InvalidName, request.ProfileInformation.FirstName))
                .And(() => request.ProfileInformation.LastName
                    .ShouldBe(x => !string.IsNullOrEmpty(x), nameof(request.ProfileInformation.LastName), string.Format(Errors.FieldCannotBeEmpty, nameof(request.ProfileInformation.LastName)))
                    .ThenShouldBe(x => x.IsValidName(), nameof(request.ProfileInformation.LastName), string.Format(Errors.InvalidName, request.ProfileInformation.LastName)))
                .And(() => request.ProfileInformation.DateOfBirth
                    .ShouldBe(x => x.IsValidDob(), nameof(request.ProfileInformation.DateOfBirth), Errors.InvalidDob));

            return secondAttempt;
        }
    }
}