using Library.Core.Domain;
using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Service;

public class MediaPlayerService
{
    private readonly IMediaRepository _mediaRepo;
    private readonly IUnitOfWork _uow;

    // Constructor Injection: Service đòi hỏi Interface, không quan tâm ai thực thi nó
    public MediaPlayerService(IMediaRepository mediaRepo, IUnitOfWork uow)
    {
        _mediaRepo = mediaRepo;
        _uow = uow;
    }

    public async Task AddVideoAsync(string title, string path)
    {
        var media = new MediaFile
        {
            Id = Guid.NewGuid(),
            Title = title,
            FilePath = path
        };

        await _mediaRepo.AddAsync(media);
        await _uow.SaveChangesAsync();
    }

    public async Task ChangeVideoTitleAsync(Guid id, string newTitle)
    {
        var media = await _mediaRepo.GetByIdAsync(id);
        if (media != null)
        {
            media.Title = newTitle;
            await _mediaRepo.UpdateAsync(media);
            await _uow.SaveChangesAsync();
        }
    }

    public async Task PlayAsync(Guid id)
    {
        var media = await _mediaRepo.GetByIdAsync(id);
        if (media != null)
        {
            media.IsPlaying = true;
            await _uow.SaveChangesAsync();
            // Trong thực tế, logic phát video có thể gọi 1 service khác ở đây
            Console.WriteLine($"\n[SERVICE LOG] Đang phát: {media.Title} ({media.FilePath})");
        }
    }

    public async Task<IEnumerable<MediaFile>> GetLibraryAsync()
    {
        return await _mediaRepo.GetAllAsync();
    }
    public async Task<MediaFile?> GetByIdAsync(Guid id)
    {
        return await _mediaRepo.GetByIdAsync(id);
    }
}