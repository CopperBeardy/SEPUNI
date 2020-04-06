using AutoMapper;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Areas.Staff.Models.ViewModels;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Repositories;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff, Admin")]
    public class ListingController : Controller
    {



        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        readonly IHostEnvironment _hostEnvironment;
        readonly IImageUploadWrapper _imageUploadWrapper;

        public ListingController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper,
            IMapper mapper,IHostEnvironment hostEnvironment,IImageUploadWrapper imageUploadWrapper)
        {
            _imageUploadWrapper = imageUploadWrapper;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;
            
        }

        // GET: Admin/Listing
        public async Task<IActionResult> Index()
        {
            List<ListingManagementDTO> listing = _mapper.Map<List<ListingManagementDTO>>(await _repositoryWrapper.Listing.GetAllListingsAsync());

            
            return View(nameof(Index), listing);
        }

        // GET: Admin/Listing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id<= 0)
            {
                _logger.LogError($"Invalid id past to {nameof(Details)}"); 
                return RedirectToAction(nameof(Index));
            }

            ListingManagementDTO listing = _mapper.Map<ListingManagementDTO>(await _repositoryWrapper.Listing.GetListingByIdAsync(id));
           //todo change address tostring representation of the address. 

            return View(nameof(Details), listing);
        }

        // GET: Admin/Listing/Create
        public async Task<IActionResult> Create()
        {

            ViewData["AddressId"] = await GetAddresses(null);
           
            return View(nameof(Create));
        }

        private async Task<List<SelectListItem>> GetAddresses(int? id)
        {
            List<Address> addressEntities = await _repositoryWrapper.Address.GetAllAddressesNotInUseAsync();
            List<SelectListItem> addresses = new List<SelectListItem>();
            foreach (Address address in addressEntities)
            {
                var addy = $"{address.Number}, {address.FirstLine}, {address.TownCity}, {address.Postcode}";
                if (id == address.Id)
                {
                    addresses.Add(new SelectListItem(addy, $"{address.Id}", true));
                }
                else
                {
                    addresses.Add(new SelectListItem(addy, $"{address.Id}", false));

                }
            }

            return addresses;
        }

        // POST: Admin/Listing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingManagementDTO listingManagementDTO)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var mapped = _mapper.Map<Listing>(listingManagementDTO);
                    mapped.Image = new Photo()
                    {
                        ImageLink = _imageUploadWrapper.Upload(listingManagementDTO.File, _hostEnvironment)
                    };
             
                    _repositoryWrapper.Listing.CreateListing(mapped);
                    await _repositoryWrapper.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating listing: {ex}");
                }
            }
            ViewData["AddressId"] = await GetAddresses(null);
            return View(nameof(Create), listingManagementDTO);
        }

        // GET: Admin/Listing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"Invalid id past to {nameof(Edit)}");
                return RedirectToAction(nameof(Details));
            }

            var listing = _mapper.Map<ListingManagementDTO>(await _repositoryWrapper.Listing.GetListingByIdAsync(id));



            ViewData["AddressId"] = await GetAddresses(listing.Address.Id);
            return View(nameof(Edit), listing);
        }

        // POST: Admin/Listing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ListingManagementDTO listingManagementDTO)
        {
            if (id != listingManagementDTO.Id)
             
                {
                    _logger.LogError($"Id did not match for to {nameof(Edit)}.  Expected:{id}, Actual{listingManagementDTO.Id}");
                    return RedirectToAction(nameof(Index));
                }

           
            if (ModelState.IsValid)
            {
                try
                {
                    var mapped = _mapper.Map<Listing>(listingManagementDTO);
                    if(listingManagementDTO.File != null)
                    {
                        mapped.Image = new Photo()
                        {
                            ImageLink = _imageUploadWrapper.Upload(listingManagementDTO.File, _hostEnvironment)
                        };
                    } else
                    {
                       var listing = await _repositoryWrapper.Listing.GetListingByIdAsync(id);
                        mapped.Image = listing.Image;
                        mapped.ImageId = listing.ImageId;
                    }
                    
               
                    mapped.Address = await _repositoryWrapper.Address.GetAddressByIdAsync(listingManagementDTO.AddressId);

                    _repositoryWrapper.Listing.UpdateListing(mapped);
           
                    await _repositoryWrapper.SaveAsync();

                }
                catch (Exception ex)
                {
                    
                        _logger.LogError($"Error updating listing: {ex}");
                       
                 
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = await GetAddresses(listingManagementDTO.Address.Id);
            return View(nameof(Edit), listingManagementDTO);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"invaliud value passed to Listing controller, method : {nameof(Delete)}");
              return  RedirectToAction(nameof(Index));
            }

            var listing = _mapper.Map<ListingManagementDTO>(await _repositoryWrapper.Listing.GetListingByIdAsync(id));
           

            return View(nameof(Delete), listing);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var listing = await _repositoryWrapper.Listing.GetListingByIdAsync(id);

                _repositoryWrapper.Listing.DeleteListing(listing);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"unable to delete Listing: {ex}");
                return RedirectToAction(nameof(Delete), id);
            }
        }

       

    }
}
