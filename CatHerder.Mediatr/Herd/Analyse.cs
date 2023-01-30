using CatHerder.Data;
using CatHerder.Data.Entities;
using MediatR;

namespace CatHerder.Mediatr.Herd;

public class Analyse : IRequestHandler<Analyse.Request, ISlot[]>
{
    private readonly ICrud _crud;

    public Analyse(ICrud crud)
    {
        _crud = crud;
    }

    public async Task<ISlot[]> Handle(Request request, CancellationToken cancellationToken)
    {
        var herd = await _crud.GetAsync<CatHerder.Data.Entities.Herd>(request.PublicId, cancellationToken);
        var herdModel = await herd.FromEntity(_crud, cancellationToken);
        var catCount = herdModel.Cats!.Count();
        var futureEntries = herdModel.Calendar!.Slots!.Where(s => s.From > DateTime.UtcNow).ToList();
        var scoredEntries = futureEntries.Select(e => (e.CatEventModels?.Sum(GetScore) ?? 0, e)).ToList();
        var bestSlots = scoredEntries.OrderBy(s => s.Item1).ThenBy(s => s.e.From);
        return bestSlots.Select(b => b.e).ToArray();
        int GetScore(CatEventModel catEventModel)
        {
            if (catEventModel == null)
            {
                return 0;
            }
            else
            {
                if (catEventModel.Response == Response.Yes)
                {
                    return 3;
                }
                else if (catEventModel.Response == Response.Maybe)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class Request : IRequest<ISlot[]>
    {
        public Guid PublicId { get; set; }
    }
}
