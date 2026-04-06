namespace DeveloperHomeworkAssignment.API.Messaging.User
{
    public interface IUserMessagePublisher
    {
        void PublishUserCreated<TIn>(TIn user, Func<TIn, UserCreatedMessage> map);
    }
}