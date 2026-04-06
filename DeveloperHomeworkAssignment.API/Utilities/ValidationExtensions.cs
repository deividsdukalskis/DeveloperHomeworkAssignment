namespace DeveloperHomeworkAssignment.API.Utilities
{
    public static class ValidationExtensions
    {
        public static Result<TIn, ValidationError> ShouldBe<TIn>(this TIn field, Func<TIn, bool> check, string fieldName, string ErrorText)
        {
            bool result = check(field);
            if (result)
            {
                return Result<TIn, ValidationError>.Success(field);
            }

            return Result<TIn, ValidationError>.Failure(new ValidationError(fieldName, ErrorText));
        }

        public static Result<ErrorCollection<ValidationError>> And(this Result<ValidationError> previous, Func<Result<ValidationError>> current)
        {
            ErrorCollection<ValidationError> errors = new ErrorCollection<ValidationError>();
            var currentEvaluated = current();
            if (previous.IsSuccess && currentEvaluated.IsSuccess)
            {
                return Result<ErrorCollection<ValidationError>>.Success();
            }

            if (!previous.IsSuccess)
            {
                errors.Add(previous.Error);
            }

            if (!currentEvaluated.IsSuccess)
            {
                errors.Add(currentEvaluated.Error);
            }

            return Result<ErrorCollection<ValidationError>>.Failure(errors);
        }

        public static Result<ErrorCollection<ValidationError>> And(this Result<ErrorCollection<ValidationError>> previous, Func<Result<ValidationError>> current)
        {
            ErrorCollection<ValidationError> errors = new ErrorCollection<ValidationError>();
            var currentEvaluated = current();
            if (!previous.IsSuccess)
            {
                errors = previous.Error;
            }

            if (previous.IsSuccess && currentEvaluated.IsSuccess)
            {
                return Result<ErrorCollection<ValidationError>>.Success();
            }

            if (!currentEvaluated.IsSuccess)
            {
                errors.Add(currentEvaluated.Error);
            }

            return Result<ErrorCollection<ValidationError>>.Failure(errors);
        }

        public static Result<TIn, ValidationError> ThenShouldBe<TIn>(this Result<TIn, ValidationError> previous, Func<TIn, bool> check, string fieldName, string ErrorText)
        {
            if (!previous.IsSuccess)
            {
                return previous;
            }

            bool result = check(previous.Value);
            if (result)
            {
                return Result<TIn, ValidationError>.Success(previous.Value);
            }

            return Result<TIn, ValidationError>.Failure(new ValidationError(fieldName, ErrorText));
        }
    }
}