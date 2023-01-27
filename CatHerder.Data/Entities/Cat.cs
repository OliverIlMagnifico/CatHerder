using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatHerder.Data.Entities;

public class Cat : IHasId
{
    [BsonId]
    public ObjectId Id { get; set; }

    public Guid PublicId { get; set; }
    public string? Name { get; set; }
}
