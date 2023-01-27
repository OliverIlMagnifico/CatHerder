using MongoDB.Bson;
using MongoDB.Driver;

namespace CatHerder.Data;

public class MongoCrud : ICrud
{
    private readonly MongoContext _context;

    public MongoCrud(MongoContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync<T>(T toAdd, CancellationToken cancellationToken)
    {
        var collection = _context.GetCollection<T>();
        await collection.InsertOneAsync(toAdd, null, cancellationToken);
        return toAdd;
    }

    public async Task<long> CountAsync<T>(CancellationToken cancellationToken)
    {
        var f = Builders<T>.Filter;

        var collection = _context.GetCollection<T>();
        return await collection.CountDocumentsAsync(f.Empty, cancellationToken: cancellationToken);
    }

    public async Task<T> GetAsync<T>(ObjectId id, CancellationToken cancellationToken) where T : IHasId
    {
        var idFilter = Builders<T>.Filter.Eq(x => x.Id, id);
        var collection = _context.GetCollection<T>();
        var result = await collection.FindAsync<T>(idFilter, null, cancellationToken);
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<T>> GetAsync<T>(CancellationToken cancellationToken, int skip = 0, int take = 50, params (string field, string value)[] filter)
    {
        var filterDefinition = Builders<T>.Filter.And();

        foreach (var component in filter)
        {
            var th = Builders<T>.Filter.Eq(component.field, component.value);
            filterDefinition = filterDefinition & th;
        }

        var collection = _context.GetCollection<T>();
        return await collection.Find<T>(filterDefinition, null).Skip(skip).Limit(take).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetPageAsync<T>(CancellationToken cancellationToken, int skip = 0, int take = 50)
    {
        var f = Builders<T>.Filter;

        var collection = _context.GetCollection<T>();
        return await collection.Find(f.Empty).Skip(skip).Limit(take).ToListAsync(cancellationToken);
    }
}
