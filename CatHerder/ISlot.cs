namespace CatHerder;

public interface ISlot
{
    DateTime From { get; set; }
    DateTime To { get; set; }
    string? Id { get; set; }
    List<CatEventModel>? CatEventModels { get; set; }
}
