using AutoMapper;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models;
using CordEstates.Models.DTOs;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
        public async Task<IActionResult> Index(int pageNumber =1)
        {
            try
            {
                var data = _mapper.Map<List<ExtendedListingDTO>>(await _repositoryWrapper.Listing.GetAllListingsForSaleAsync());
                IQueryable<ExtendedListingDTO> dataQuerable = data.AsQueryable();
                var model = PaginatedList<ExtendedListingDTO>.Create(dataQuerable, pageNumber, 5);
                return View(nameof(Index), model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to retrieve the Listings: {ex}");
                throw new Exception($"Unable to retrieve the Listings: {ex}");
            }
        }




        //// GET: Listing/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    ListingDetailDTO listing = new ListingDetailDTO();
        //    try
        //    {
        //        listing = _mapper.Map<ListingDetailDTO>(await _repositoryWrapper.Listing.GetListingByIdAsync(id));

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Unable to retrieve the Listing with id-{id} : {ex}");
        //        throw new Exception($"Unable to retrieve the Listing with id-{id}: {ex}");
        //    }


        //    if (listing == null)
        //    {
        //        return NotFound();
        //    }
        //    listing.Listing = _mapper.Map<Listing>(listing);
        //    return View(listing);
        //}
        //// GET: Admin/Listing/Edit/5
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

            if(User.IsInRole("Customer"))
            {
                string user = await _repositoryWrapper.Employee.GetUserId(User);
                var cust = await _repositoryWrapper.Customer.GetCustomerByUserId(user);

                var props = await _repositoryWrapper.Customer.GetCustomersPropertyAsync(cust.Id);
                var r = props.PropertiesInterestedIn
                    .Where(x => x.CustomerId.Equals(cust.Id) && x.PropertyId.Equals(listing.Id)).FirstOrDefault();
                if (r == null )
                {
                    ViewData["following"] = false;
                }
                else
                {
                    ViewData["following"] = true;
                }
           


            }
            return View(nameof(Details), listing);
        }

        // POST: Admin/Listing/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ListingDetailDTO listingDetailDTO,bool follow)
        {
            if (id != listingDetailDTO.Id)

            {
                _logger.LogError($"Id did not match for to {nameof(Edit)}.  Expected:{id}, Actual{listingDetailDTO.Id}");
                return RedirectToAction(nameof(Index));
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var mapped = _mapper.Map<Listing>(listingDetailDTO);
                    
                   var listing = await _repositoryWrapper.Listing.GetListingByIdAsync(id);
                    string user = await _repositoryWrapper.Employee.GetUserId(User);
                    await _repositoryWrapper.Customer.ToggleFollow(user, listing,follow);


                 

                    await _repositoryWrapper.SaveAsync();

                }
                catch (Exception ex)
                {

                    _logger.LogError($"Error updating listing: {ex}");


                }
                return RedirectToAction(nameof(Index));
            }
           return View(nameof(Details), listingDetailDTO);
        }
      



    }
}
