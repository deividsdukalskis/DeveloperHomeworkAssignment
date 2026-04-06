namespace DeveloperHomeworkAssignment.API.Messaging.User
{
    using Newtonsoft.Json;

    public class UserMessagePublisher : IUserMessagePublisher
    {
        public void PublishUserCreated<TIn>(TIn user, Func<TIn, UserCreatedMessage> map)
        {
            string json = JsonConvert.SerializeObject(map(user), Formatting.Indented);
            Console.WriteLine("======================");
            Console.WriteLine("UserCreated");
            Console.WriteLine("======================");
            Console.WriteLine(json);
            Console.WriteLine("======================");
        }
    }
}