namespace CatHerder;

public abstract class ASlot
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? Id { get; set; }
    public List<CatEventModel>? CatEventModels { get; set; }
}
