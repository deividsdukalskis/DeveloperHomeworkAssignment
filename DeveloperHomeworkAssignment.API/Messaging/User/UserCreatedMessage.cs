namespace DeveloperHomeworkAssignment.API.Messaging.User
{
    public record UserCreatedMessage(Guid UserId, string Username, string Email);
}