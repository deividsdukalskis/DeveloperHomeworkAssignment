namespace DeveloperHomeworkAssignment.API.Utilities
{
    public static class HttpResultExtensions
    {
        public static IResult MapToHttpResult<TError>(this ErrorCollection<TError> errors) where TError : Error
        {
            var notFoundErrors = errors.Errors.Where(error => error is NotFoundError)
                .Select(error => error as NotFoundError);
            if (notFoundErrors.Any())
            {
                return Results.NotFound(notFoundErrors);
            }

            var invalidOperationErrors = errors.Errors.Where(error => error is InvalidOperationError)
                .Select(error => error as InvalidOperationError);
            if (invalidOperationErrors.Any())
            {
                return Results.BadRequest(invalidOperationErrors);
            }

            var validationErrors = errors.Errors.Where(error => error is ValidationError)
                .Select(error => error as ValidationError);
            if (validationErrors.Any())
            {
                Dictionary<string, string[]> errorsDict = new Dictionary<string, string[]>();
                foreach (var error in validationErrors)
                {
                    errorsDict.Add(error!.Field, new string[] { error.Message });
                }

                return Results.ValidationProblem(errorsDict);
            }

            return Results.Problem();
        }

        public static IResult MapToHttpResult(this Error error)
        {
            switch (error)
            {
                case NotFoundError err:
                    return Results.NotFound(err);
                case ValidationError err:
                    var errDict = new Dictionary<string, string[]>();
                    errDict.Add(err.Field, new string[] { err.Message });
                    return Results.ValidationProblem(errDict);
                case InvalidOperationError err:
                    return Results.BadRequest(err);
                default:
                    return Results.Problem();
            }
        }
    }
}