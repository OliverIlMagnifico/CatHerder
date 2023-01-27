namespace CatHerder;

public class HerdModel
{
    public string? Id { get; set; }
    public Guid PublicId { get; set; }
    public string? Name { get; set; }

    public CalendarModel? Calendar { get; set; }

    public List<CatModel>? Cats { get; set; }
}
