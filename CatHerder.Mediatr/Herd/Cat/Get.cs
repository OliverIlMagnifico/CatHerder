using CatHerder.Data;
using CatHerder.Mediatr.Pipelines;
using FluentValidation;
using MediatR;

namespace CatHerder.Mediatr.Herd.Cat;

public class Get : IRequestHandler<Get.Request, CatModel>
{
    private readonly ICrud _crud;

    public Get(ICrud crud)
    {
        _crud = crud;
    }

    public async Task<CatModel> Handle(Request request, CancellationToken cancellationToken)
    {
        var cat = await _crud.GetAsync<Data.Entities.Cat>(request.PublicId, cancellationToken);

        return cat.FromEntity();
    }

    public class Request : IRequest<CatModel>
    {
        public Guid PublicId { get; set; }
    }

    public class Validation : ValidationBase<Request>, IValidationHandler<Request, CatModel>
    {
        public Validation()
        {
            RuleFor(x => x.PublicId).NotEqual(new Guid());
        }
    }
}
