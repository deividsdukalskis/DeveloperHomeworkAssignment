namespace DeveloperHomeworkAssignment.API.Utilities
{
    public static class NullableExtensions
    {
        public static TOut EvaluateNullable<TIn, TOut>(this TIn? nullable, Func<TOut> onNull, Func<TIn, TOut> onValue)
        {
            if (nullable == null)
            {
                return onNull();
            }

            return onValue(nullable);
        }
    }
}