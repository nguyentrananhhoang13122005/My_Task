using Library.Core.Domain;
using Library.Core.Interfaces;

namespace Library.Infrastructure.InMemory;

public class InMemoryMediaRepository : IMediaRepository
{
        private static readonly List<MediaFile> _database = new();

    public Task<IEnumerable<MediaFile>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<MediaFile>>(_database);
    }

    public Task<MediaFile?> GetByIdAsync(Guid id)
    {
        var item = _database.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(item);
    }

    public Task AddAsync(MediaFile media)
    {
        _database.Add(media);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(MediaFile media)
    {
        
        return Task.CompletedTask;
    }
}