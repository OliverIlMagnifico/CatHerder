using CatHerder.Data;
using CatHerder.Mediatr.Pipelines;
using FluentValidation;
using MediatR;

namespace CatHerder.Mediatr.Herd;

public class Get : IRequestHandler<Get.Request, HerdModel>
{
    private readonly ICrud _crud;

    public Get(ICrud crud)
    {
        _crud = crud;
    }

    public async Task<HerdModel> Handle(Request request, CancellationToken cancellationToken)
    {
        var herd = await _crud.GetAsync<CatHerder.Data.Entities.Herd>(request.PublicId, cancellationToken);

        return await herd.FromEntity(_crud, cancellationToken);
    }

    public class Request : IRequest<HerdModel>
    {
        public Guid PublicId { get; set; }
    }

    public class Validation : ValidationBase<Request>, IValidationHandler<Request, HerdModel>
    {
        public Validation()
        {
            RuleFor(x => x.PublicId).NotEqual(new Guid());
        }
    }
}
