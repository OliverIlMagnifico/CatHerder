using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatHerder.Data.Entities;

public class CatEvent : IHasId
{
    [BsonId]
    public ObjectId Id { get; set; }

    public ObjectId CatId { get; set; }

    public Response Response { get; set; } = Response.NotSet;
}
