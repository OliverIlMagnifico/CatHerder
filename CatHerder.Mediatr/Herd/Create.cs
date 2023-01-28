using CatHerder.Data;
using CatHerder.Data.Entities;
using CatHerder.Mediatr.Pipelines;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using System;

namespace CatHerder.Mediatr.Herd;

public class Create : IRequestHandler<Create.Request, (Guid herdId, Guid catId)>
{
    private readonly ICrud? _crud;

    public Create(ICrud? crud)
    {
        _crud = crud;
    }

    public async Task<(Guid herdId, Guid catId)> Handle(Request request, CancellationToken cancellationToken)
    {
        var slots = Get(request);
        await Task.WhenAll(slots.Select(s => Task.Run(async () => await _crud!.AddAsync<Slot>(s, cancellationToken))));
        
        var calendar = new Data.Entities.Calendar()
        {
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddMonths(request.Months),
            SlotIds = slots.Select(s => s.Id).ToList()
        };
        await _crud!.AddAsync<Data.Entities.Calendar>(calendar, cancellationToken);
        
        var firstCat = new Data.Entities.Cat
        {
            Name = request.FirstCat,
            PublicId = Guid.NewGuid(),
        };
        await _crud.AddAsync(firstCat, cancellationToken);   
        
        var herd = new CatHerder.Data.Entities.Herd()
        {
            Name = request.Name,
            PublicId = Guid.NewGuid(),
            CalendarId = calendar.Id,
            CatIds = new List<ObjectId> { firstCat.Id }
        };
        await _crud.AddAsync(herd, cancellationToken);

        return (herd.PublicId, firstCat.PublicId);
    }

    private static Slot[] Get(Request request)
    {
        var result = new List<Slot>();
        var dateNow = DateTime.UtcNow.Date;
        var dateEnd = dateNow.AddMonths(request.Months);
        while (dateNow < dateEnd)
        {
            dateNow = dateNow.AddDays(1);

            switch (dateNow.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (request.Sunday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Monday:
                    if (request.Monday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (request.Tuesday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (request.Wednesday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (request.Thursday) { 
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Friday:
                    if (request.Friday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (request.Saturday)
                    {
                        result.Add(GetSlot(dateNow));
                    }
                    break;
            }
        }
        return result.ToArray();

        Slot GetSlot(DateTime start)
        {
            return new Slot
            {
                Start = start,
                End = start.AddDays(1).AddSeconds(-1),
                CatEvents = new List<ObjectId>(0)
            };
        }
    }

    public class Request : IRequest<(Guid herdId, Guid catId)>
    {
        public string? Name { get; set; }
        public int Months { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set;}
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public string? FirstCat { get; set; }
    }

    public class Validation : ValidationBase<Request>, IValidationHandler<Request, (Guid herdId, Guid catId)>
    {
        public Validation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("You must give your herd a name");
            RuleFor(x => x.Months).GreaterThan(0).LessThan(4).WithMessage("You must create specify a period of months, between 0 and 3");
            RuleFor(x => x.FirstCat).NotNull().NotEmpty().WithMessage("You must create a cat when you create a herd.");
            RuleFor(x => x).Must(r => r.Sunday || r.Saturday || r.Monday || r.Tuesday || r.Wednesday || r.Thursday || r.Friday).WithMessage("Select at least one day");
        }
    }
}
