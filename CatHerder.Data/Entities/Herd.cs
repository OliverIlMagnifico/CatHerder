using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatHerder.Data.Entities;
public class Herd : IHasId
{
    [BsonId]
    public ObjectId Id { get; set; }
    public Guid PublicId { get; set; }
    public string? Name { get; set; }

    public List<ObjectId>? CatIds { get; set; }

    public ObjectId? CalendarId { get; set; }
}
