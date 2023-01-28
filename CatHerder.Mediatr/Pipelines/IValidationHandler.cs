using FluentValidation.Results;
using MediatR;

namespace CatHerder.Mediatr.Pipelines
{
    public interface IValidationHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<ValidationResult> Handle(TRequest request, CancellationToken cancellationToken);
    }
}