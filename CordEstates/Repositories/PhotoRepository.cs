using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class PhotoRepository : RepositoryBase<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context) { }


        //TODO also remove from storage
        public void DeletePhoto(Photo photo) => Delete(photo);

        public bool Exists(int id) => _context.Photos.Any(x => x.Id.Equals( id));       

        public async Task<List<Photo>> GetAllPhotosAsync() => await FindAll().ToListAsync();
        public async Task<Photo> GetPhotoByIdAsync(int id) => await FindByCondition(x=>x.Id.Equals(id)).FirstOrDefaultAsync();

        public void UploadPhoto(Photo photo) => Create(photo);
    }
}
