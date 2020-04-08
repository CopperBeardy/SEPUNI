using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Models.DTOs
{
    public class PhotoDTO
    {
        public int Id { get; set; }


        public string ImageLink { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
