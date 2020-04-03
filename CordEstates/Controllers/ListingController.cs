using AutoMapper;
using CordEstates.Helpers;
using CordEstates.Models.DTOs;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Controllers
{
    public class ListingController : Controller
    {



        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public ListingController(ILoggerManager logger
            , IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Listing
        public async Task<IActionResult> Index()
        {
            try
            {
                var listings = _mapper.Map<List<ExtendedListingDTO>>(await _repositoryWrapper.Listing.GetAllListingsForSaleAsync());
                return View(nameof(Index), listings);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to retrieve the Listings: {ex}");
                throw new Exception($"Unable to retrieve the Listings: {ex}");
            }
        }




        // GET: Listing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ListingDetailDTO listing = new ListingDetailDTO();
            try
            {
                listing = _mapper.Map<ListingDetailDTO>(await _repositoryWrapper.Listing.GetListingByIdAsync(id));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to retrieve the Listing with id-{id} : {ex}");
                throw new Exception($"Unable to retrieve the Listing with id-{id}: {ex}");
            }


            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }


    }
}
