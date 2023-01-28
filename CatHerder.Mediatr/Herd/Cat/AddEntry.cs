using CatHerder.Data.Entities;
using CatHerder.Mediatr.Pipelines;
using FluentValidation;
using MediatR;
using MongoDB.Bson;

namespace CatHerder.Mediatr.Herd.Cat;

public class AddEntry : IRequestHandler<AddEntry.Request>
{
    private readonly ICrud _crud;

    public AddEntry(ICrud crud)
    {
        _crud = crud;
    }

    public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
    {
        var objectCatId = ObjectId.Parse(request.CatId);
        var slot = await _crud.GetAsync<Slot>(ObjectId.Parse(request.SlotId), cancellationToken);

        foreach (var modelId in slot!.CatEvents ?? new List<ObjectId>(0))
        {
            var catEvent = await _crud.GetAsync<CatEvent>(modelId, cancellationToken);

            if (catEvent.CatId == objectCatId && catEvent.Response != request.Response)
            {
                catEvent.Response = request.Response;
                await _crud.UpdateAsync<CatEvent>(catEvent, cancellationToken);
                break;
            }
        }

        return Unit.Value;
    }

    public class Request : IRequest
    {
        public CatHerder.Response Response { get; set; }

        public string? CatId { get; set; }
        public string? SlotId { get; set; }
    }

    public class Validation : ValidationBase<Request>, IValidationHandler<Request, Unit>
    {
        public Validation()
        {
            RuleFor(x => x.SlotId).NotNull().NotEmpty();
            RuleFor(x => x.CatId).NotNull().NotEmpty();
            RuleFor(x => x.Response).NotEqual(Response.NotSet);
        }
    }
}


