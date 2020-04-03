using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IPhotoRepository : IRepositoryBase<Photo>
    {
        Task<List<Photo>> GetAllPhotosAsync();
         
        void UploadPhoto(Photo photo);
        void DeletePhoto(Photo photo);

        Task<Photo> GetPhotoByIdAsync(int id);
        bool Exists(int id);
    }
}
