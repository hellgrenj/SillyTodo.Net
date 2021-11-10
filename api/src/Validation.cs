using FluentValidation.Results;
public interface IValidationWrapper
{
    Task<IResult> ValidateAndSend<T>(IValidator<T> validator, T cmd);
}
public class ValidationWrapper : IValidationWrapper
{
    private readonly IMediator _mediator;
    public ValidationWrapper(IMediator mediator) => _mediator = mediator;
    public async Task<IResult> ValidateAndSend<T>(IValidator<T> validator, T cmd)
    {
        var validationResult = validator.Validate(cmd);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        return Results.Ok(await _mediator.Send(cmd));
    }
}
public static class ValidationExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
        => validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );
}
