using CatHerder.Mediatr.Pipelines;
using MediatR;
using System.Text;
using System.Threading.Tasks;

namespace CatHerder.Mediatr.Pipelines
{
    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                     where TRequest : IRequest<TResponse>
    {
        public ValidationPipeline(IEnumerable<IValidationHandler<TRequest, TResponse>> processors)
        {
            Processors = processors;
        }

        public IEnumerable<IValidationHandler<TRequest, TResponse>> Processors { get; }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var processor in Processors)
            {
                var validationResult = await processor.Handle(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    throw new Validation.ValidationException(validationResult);
                }
            }

            var nextResult = await next();

            return nextResult;
        }
    }
}
