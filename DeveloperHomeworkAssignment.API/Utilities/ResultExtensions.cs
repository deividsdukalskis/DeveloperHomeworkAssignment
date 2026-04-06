namespace DeveloperHomeworkAssignment.API.Utilities
{
    public static class ResultExtensions
    {
        public static TResult Evaluate<TInError, TResult>(this Result<TInError> result, Func<TResult> onSuccess, Func<TInError, TResult> onFailure)
        {
            if (result.IsSuccess)
            {
                return onSuccess();
            }

            return onFailure(result.Error);
        }
    }
}