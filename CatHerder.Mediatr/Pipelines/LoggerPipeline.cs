using MediatR;
using Microsoft.Extensions.Logging;
namespace CatHerder.Mediatr.Pipelines
{
    public class LoggerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                 where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        public LoggerPipeline(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogDebug("Sending {type} ", request.GetType());

            var nextResult = await next();

            return nextResult;
        }
    }
}
