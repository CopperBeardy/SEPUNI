using AutoMapper;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models;
using CordEstates.Models.DTOs;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Admin")]
    public class PhotoController : Controller
    {



        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly IImageUploadWrapper _imageUploadWrapper;
        readonly ILoggerManager _logger;
        readonly IHostEnvironment _hostEnvironment;

        public PhotoController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper, IHostEnvironment hostEnvironment, IImageUploadWrapper imageUploadWrapper)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _imageUploadWrapper = imageUploadWrapper;
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;

        }

        // GET: Admin/Photo
        public async Task<IActionResult> Index(int pageNumber =1 )
        {

            var  data = _mapper.Map<List<PhotoDTO>>(await _repositoryWrapper.Photo.GetAllPhotosAsync());
            IQueryable<PhotoDTO> dataQuerable = data.AsQueryable();
            var model = PaginatedList<PhotoDTO>.Create(dataQuerable, pageNumber, 5);


            return View(nameof(Index), model);
        }


        // GET: Admin/Photo/Create
        public IActionResult CreatePhoto()
        {
            return View(nameof(CreatePhoto));
        }

        // POST: Admin/Photo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhoto(PhotoDTO photo)
        {
            if (ModelState.IsValid)
            {
                photo.ImageLink = _imageUploadWrapper.Upload(photo.File, _hostEnvironment);

                _repositoryWrapper.Photo.Create(_mapper.Map<Photo>(photo));

                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(CreatePhoto), photo);
        }


        // GET: Admin/Photo/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Invalid ID: {id} was passed to {nameof(Delete)} in the PhotoController ");
                return RedirectToAction(nameof(Index));
            }

            PhotoDTO photo = _mapper.Map<PhotoDTO>(await _repositoryWrapper.Photo.GetPhotoByIdAsync(id));


            return View(nameof(Delete), photo);
        }

        // POST: Admin/Photo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Photo photo = await _repositoryWrapper.Photo.GetPhotoByIdAsync(id);

                _repositoryWrapper.Photo.DeletePhoto(photo);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"unable to delete Photo: {ex}");
                return RedirectToAction(nameof(Delete), id);
            }
        }


    }
}
