namespace DeveloperHomeworkAssignment.API.Features
{
    using DeveloperHomeworkAssignment.API.DataAccess;
    using DeveloperHomeworkAssignment.API.Utilities;
    using Microsoft.AspNetCore.Mvc;

    public static class GetUserWithProfileInformation
    {
        public static readonly string Uri = "/users/{userId}";

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

        public static IResult Endpoint([FromRoute]Guid userId, DHADbContext dbContext)
        {
            return dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new Response(
                    u.Id,
                    u.Username,
                    u.Email,
                    new Response.Profile(
                        u.Profile.FirstName,
                        u.Profile.LastName,
                        u.Profile.DateOfBirth)))
                .FirstOrDefault()
                .EvaluateNullable(onNull: () =>
                {
                    return Results.NotFound(string.Format(Errors.UserNotFound, userId));
                },
                onValue: (value) =>
                {
                    return Results.Ok(value);
                });
        }
    }
}