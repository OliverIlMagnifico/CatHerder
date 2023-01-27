namespace CatHerder;

public class CalendarModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public HashSet<ISlot>? Slots { get; set; }
}
