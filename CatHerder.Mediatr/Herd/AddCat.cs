using CatHerder.Mediatr.Pipelines;
using FluentValidation;
using MediatR;

namespace CatHerder.Mediatr.Herd;

public class AddCat : IRequestHandler<AddCat.Request, Guid>
{
    private readonly ICrud _crud;

    public AddCat(ICrud crud)
    {
        _crud = crud;
    }

    public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
    {
        var cat = new CatHerder.Data.Entities.Cat
        {
            Name = request.Name,
            PublicId = Guid.NewGuid()
        };

        await _crud.AddAsync(cat, cancellationToken);

        var herd = await _crud.GetAsync<CatHerder.Data.Entities.Herd>(request.HerdPublicId, cancellationToken);

        herd!.CatIds!.Add(cat.Id);

        await _crud.UpdateAsync(herd, cancellationToken);

        return cat.PublicId;
    }

    public class Request : IRequest<Guid>
    {
        public string? Name { get; set; }

        public Guid HerdPublicId { get; set; }
    }

    public class Validation : ValidationBase<Request>, IValidationHandler<Request, Guid>
    {
        public Validation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
