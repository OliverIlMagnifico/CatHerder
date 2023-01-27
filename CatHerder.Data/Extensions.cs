using CatHerder.Data.Entities;
using MongoDB.Bson;

namespace CatHerder.Data;
public static class Extensions
{
    public static async Task<CalendarModel> FromEntity(this Calendar calendar, ICrud crud, CancellationToken cancellationToken)
    {
        var result = new CalendarModel()
        {
            Name = calendar.Name,
            Start = calendar.Start,
            End = calendar.End,
            Id = calendar.Id.ToString(),
            Slots = new HashSet<ISlot>(await (calendar?.SlotIds ?? new List<ObjectId>(0)).SelectAsync(async id =>
            {
                var slots = await crud.GetAsync<Slot>(id, cancellationToken);
                var model = await slots.FromEntity(crud, cancellationToken);
                return model;
            }))
        };

        return result;
    }

    public static async Task<ISlot> FromEntity(this Slot slot, ICrud crud, CancellationToken cancellationToken)
    {
        var result = new DayModel()
        {
            From = slot.Start,
            To = slot.End,
            CatEventModels = (await (slot?.CatEvents ?? new List<ObjectId>(0)).SelectAsync(async id =>
            {
                var catEvent = await crud.GetAsync<CatEvent>(id, cancellationToken);
                var model = await catEvent.FromEntity(crud, cancellationToken);
                return model;
            })).ToList()
        };

        return result;
    }

    public static async Task<CatEventModel> FromEntity(this CatEvent catEvent, ICrud crud, CancellationToken cancellationToken)
    {
        return new CatEventModel()
        {
            Cat = (await crud.GetAsync<Cat>(catEvent.CatId, cancellationToken)).FromEntity(),
            Response = catEvent.Response,
        };
    }

    public static CatModel FromEntity(this Cat cat)
    {
        return new CatModel()
        {
            Id = cat.Id.ToString(),
            PublicId = cat.PublicId,
            Name = cat.Name,
        };
    }

    public static async Task<HerdModel> FromEntity(this Herd herd, ICrud crud, CancellationToken cancellationToken)
    {
        CalendarModel? calendarModel = null;

        if (herd.CalendarId.HasValue)
        {
            var calendar = await crud.GetAsync<Calendar>(herd.CalendarId.Value, cancellationToken);
            calendarModel = await calendar.FromEntity(crud, cancellationToken);
        }

        var result = new HerdModel()
        {
            Id = herd.Id.ToString(),
            PublicId = herd.PublicId,
            Name = herd.Name,
            Calendar = calendarModel,
            Cats = (await (herd?.CatIds ?? new List<ObjectId>(0)).SelectAsync(async id =>
            {
                var cat = await crud.GetAsync<Cat>(id, cancellationToken);
                var model = cat.FromEntity();
                return model;
            })).ToList()
        };

        return result;
    }

    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
    this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method)
    {
        return await Task.WhenAll(source.Select(async s => await method(s)));
    }
}
