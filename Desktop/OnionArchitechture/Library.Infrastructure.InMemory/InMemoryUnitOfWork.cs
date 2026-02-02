using Library.Core.Interfaces;

namespace Library.Infrastructure.InMemory;

public class InMemoryUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        
        Console.WriteLine(" [DB LOG] Đã lưu thay đổi .");
        return Task.CompletedTask;
    }
}