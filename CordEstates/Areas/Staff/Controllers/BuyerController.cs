using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Entities;
using AutoMapper;
using CordEstates.Wrappers.Interfaces;
using CordEstates.Helpers;
using Microsoft.AspNetCore.Authorization;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Models;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Admin")]
    public class BuyerController : Controller
    {

        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public BuyerController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Staff/Buyer
        public async Task<IActionResult> Index(string sortOrder,int pageNumber =1)
        {
            var data = _mapper.Map<List<BuyerManagementDTO>>(await _repositoryWrapper.Buyer.GetAllBuyersAsync());
            IQueryable<BuyerManagementDTO> sorted= data.AsQueryable();

            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["FirstNameSortParm"] = sortOrder == "First Name" ? "first_name_desc" : "First Name";
            ViewData["LastNameSortParm"] = sortOrder == "Last Name" ? "last_name_desc" : "Last Name";
            ViewData["HouseNumberSortParm"] = sortOrder == "House Number" ? "house_number_desc" : "House Number";
            ViewData["FirstLineSortParm"] = sortOrder == "First Line" ? "first_line_desc" : "First Line";
            ViewData["PostCodeSortParm"] = sortOrder == "PostCode" ? "postcode_desc" : "PostCode";
            ViewData["PhoneNumberSortParm"] = sortOrder == "Phone Number" ? "phone_number_desc" : "Phone Number";
            ViewData["currentSort"] = sortOrder;
            sorted = SortList(sortOrder, sorted);
            var model = PaginatedList<BuyerManagementDTO>.Create(sorted, pageNumber, 5);
            return View(nameof(Index),model);
        }

        private static IQueryable<BuyerManagementDTO> SortList(string sortOrder, IQueryable<BuyerManagementDTO> sorted)
        {
            switch (sortOrder)
            {
               
                case "First Name":
                    sorted = sorted.OrderBy(f => f.FirstName).AsQueryable();
                    break;
                case "first_name_desc":
                    sorted = sorted.OrderByDescending(f => f.FirstName).AsQueryable();
                    break;
                case "Last Name":
                    sorted = sorted.OrderBy(ln => ln.LastName).AsQueryable();
                    break;
                case "last_name_desc":
                    sorted = sorted.OrderByDescending(l => l.LastName).AsQueryable();
                    break;               
                case "House Number":
                    sorted = sorted.OrderBy(hn => hn.HouseNumber).AsQueryable();
                    break;
                case "house_number_desc":
                    sorted = sorted.OrderByDescending(hn => hn.HouseNumber).AsQueryable();
                    break;

                case "First Line":
                    sorted = sorted.OrderBy(fl => fl.FirstLine).AsQueryable();
                    break;
                case "first_line_desc":
                    sorted = sorted.OrderByDescending(fl => fl.FirstLine).AsQueryable();
                    break;
                case "Postcode":
                    sorted = sorted.OrderBy(pc => pc.Postcode).AsQueryable();
                    break;
                case "postcode_desc":
                    sorted = sorted.OrderByDescending(pc => pc.Postcode).AsQueryable();
                    break;
                case "PhoneNumber":
                    sorted = sorted.OrderBy(pn => pn.PhoneNumber).AsQueryable();
                    break;
                case "Phone_number_desc":
                    sorted = sorted.OrderByDescending(pn => pn.PhoneNumber).AsQueryable();
                    break;


                case "title_desc":
                    sorted = sorted.OrderByDescending(t => t.Title).AsQueryable();
                    break;
                default:
                    sorted = sorted.OrderBy(t => t.Title ).AsQueryable();
                    break;
            }

            return sorted;
        }

        // GET: Staff/Buyer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"{id} is not a valid id");
                return RedirectToAction(nameof(Index));
            }

            var buyer = _mapper.Map<BuyerManagementDTO>(await _repositoryWrapper.Buyer.GetBuyerByIdAsync(id));
               
            

            return View(nameof(Details),buyer);
        }

        // GET: Staff/Buyer/Create
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        // POST: Staff/Buyer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuyerManagementDTO buyer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.Buyer.CreateBuyer(_mapper.Map<Buyer>(buyer));
                    await _repositoryWrapper.SaveAsync();   
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured when creating a new buyer: {ex}");
                    RedirectToAction(nameof(Index));
                }       
            }
            return View(nameof(Create),buyer);
        }

        // GET: Staff/Buyer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"{id} is not a valid id");
                return RedirectToAction(nameof(Index));
            }

            var buyer = _mapper.Map<BuyerManagementDTO>(await _repositoryWrapper.Buyer.GetBuyerByIdAsync(id));
                     
            return View(nameof(Edit),buyer);
        }

        // POST: Staff/Buyer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,BuyerManagementDTO buyer)
        {
            if (id != buyer.Id)
            {
                _logger.LogError($"{id} is not a valid id");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.Buyer.UpdateBuyer(_mapper.Map<Buyer>(buyer));
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)           
                {
                    _logger.LogError($"Error occured when updating buyer: {ex}");
                  
                }
            
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit),buyer);
        }

        // GET: Staff/Buyer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <=0)
            {
                _logger.LogError($"{id} is not a valid id");
                return RedirectToAction(nameof(Index));
            }

            var buyer = _mapper.Map<BuyerManagementDTO>(await _repositoryWrapper.Buyer.GetBuyerByIdAsync(id));


            return View(nameof(Delete),buyer);
        }

        // POST: Staff/Buyer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buyer = await _repositoryWrapper.Buyer.GetBuyerByIdAsync(id);
            try
            {
_repositoryWrapper.Buyer.DeleteBuyer(buyer);
            await _repositoryWrapper.SaveAsync();
            return RedirectToAction(nameof(Index));
            }
              catch (Exception ex)
            {
                _logger.LogError($"unable to delete buyer: {ex}");
                return RedirectToAction(nameof(Delete), id);
    }
    
        }

    }
}
