namespace DeveloperHomeworkAssignment.API.Utilities
{
    using System.Collections.ObjectModel;

    public abstract record Error(string Message);
    public record NotFoundError(string Message) : Error(Message);
    public record ValidationError(string Field, string Message) : Error(Message);
    public record InvalidOperationError(string Message) : Error(Message);

    public record ErrorCollection<TError> where TError : Error
    {
        public ReadOnlyCollection<TError> Errors
        {
            get
            {
                return _errors.AsReadOnly();
            }
        }

        private readonly List<TError> _errors = new();

        public ErrorCollection<TError> Add(TError error)
        {
            _errors.Add(error);
            return this;
        }
    }
}