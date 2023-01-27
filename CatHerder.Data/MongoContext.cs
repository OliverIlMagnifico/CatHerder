using CatHerder.Data.Entities;
using MongoDB.Driver;

namespace CatHerder.Data;

public class MongoContext
{
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    public MongoContext(MongoConfiguration mongoConfiguration)
    {
        _mongoClient = new MongoClient(mongoConfiguration.ConnectionString);
        _database = _mongoClient.GetDatabase(mongoConfiguration.DatabaseName);
    }

    public IMongoCollection<Calendar> Users => _database.GetCollection<Calendar>(nameof(Calendar));
    public IMongoCollection<Cat> Cycles => _database.GetCollection<Cat>(nameof(Cat));
    public IMongoCollection<CatEvent> FieldTemplates => _database.GetCollection<CatEvent>(nameof(CatEvent));
    public IMongoCollection<Herd> CTasks => _database.GetCollection<Herd>(nameof(Herd));
    public IMongoCollection<Slot> CTaskTemplates => _database.GetCollection<Slot>(nameof(Slot));

    public IMongoCollection<T> GetCollection<T>()
    {
        return _database.GetCollection<T>(typeof(T).Name);
    }
}
