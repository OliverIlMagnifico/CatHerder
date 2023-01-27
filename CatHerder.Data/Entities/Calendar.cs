using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatHerder.Data.Entities;

public class Calendar : IHasId
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string? Name { get; set; }
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public List<ObjectId>? SlotIds { get; set; }
}
