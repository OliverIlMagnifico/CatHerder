using MongoDB.Bson;

namespace CatHerder;

public interface ICrud
{
    Task<T> AddAsync<T>(T toAdd, CancellationToken cancellationToken);
    Task<T> GetAsync<T>(ObjectId id, CancellationToken cancellationToken) where T : IHasId;
    Task<IEnumerable<T>> GetAsync<T>(CancellationToken cancellationToken, int skip = 0, int take = 50, params (string field, string value)[] filter);
    Task<IEnumerable<T>> GetPageAsync<T>(CancellationToken cancellationToken, int skip = 0, int take = 50);
    Task<long> CountAsync<T>(CancellationToken cancellationToken);
}

public interface IHasId
{
    ObjectId Id { get; }
}
