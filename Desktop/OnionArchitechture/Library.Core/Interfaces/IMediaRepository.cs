using Library.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface IMediaRepository
    {
        Task<IEnumerable<MediaFile>> GetAllAsync();
        Task<MediaFile?> GetByIdAsync(Guid id);
        Task AddAsync(MediaFile media);
        Task UpdateAsync(MediaFile media);
    }
}
