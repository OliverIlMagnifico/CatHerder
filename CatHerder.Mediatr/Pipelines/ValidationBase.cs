using FluentValidation;
using FluentValidation.Results;

namespace CatHerder.Mediatr.Pipelines
{
    public abstract class ValidationBase<FooRequest> : AbstractValidator<FooRequest>
    {
        public async Task<ValidationResult> Handle(FooRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Validate(request));
        }
    }
}
