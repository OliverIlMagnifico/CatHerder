namespace CatHerder.Mediatr.Validation
{
    using FluentValidation;
    using FluentValidation.Results;
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; private set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
