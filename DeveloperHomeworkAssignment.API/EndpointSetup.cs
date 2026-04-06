namespace DeveloperHomeworkAssignment.API
{
    using DeveloperHomeworkAssignment.API.Features;

    public static class EndpointSetup
    {
        public static WebApplication MapMinimalEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");
            api.MapPost(CreateNewUserAndProfile.Uri, CreateNewUserAndProfile.Endpoint);
            api.MapGet(GetUserWithProfileInformation.Uri, GetUserWithProfileInformation.Endpoint);
            return app;
        }
    }
}
