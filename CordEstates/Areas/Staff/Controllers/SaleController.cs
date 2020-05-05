using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CordEstates.Wrappers.Interfaces;
using CordEstates.Helpers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Models;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff, Admin")]
    public class SaleController : Controller
    {

        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public SaleController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }


        // GET: Staff/Sale
        public async Task<IActionResult> Index(string sortOrder,int pageNumber =1 )
        {
            var data = _mapper.Map<List<SaleManagementDTO>>(await _repositoryWrapper.Sale.GetAllSalesAsync());
            IQueryable<SaleManagementDTO> sorted = data.AsQueryable();
            ViewData["BuyerSortParm"] = string.IsNullOrEmpty(sortOrder) ? "buyer_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Agreed Price" ? "agreed_price_desc" : "Agreed Price";
            ViewData["SoldPropertySortParm"] = sortOrder == "Sold Property" ? "sold_property_desc" : "Sold Property";
            ViewData["currentSort"] = sortOrder;

            sorted = SortList(sortOrder, sorted);
            var model = PaginatedList<SaleManagementDTO>.Create(sorted, pageNumber, 5);
            return View(nameof(Index),model);
        }

        private static IQueryable<SaleManagementDTO> SortList(string sortOrder, IQueryable<SaleManagementDTO> sorted)
        {
            sorted = sortOrder switch
            {
                "Agreed Price" => sorted.OrderBy(ap => ap.AgreedPrice).AsQueryable(),
                "agreed_price_desc" => sorted.OrderByDescending(ap => ap.AgreedPrice).AsQueryable(),
                "Sold Property" => sorted.OrderBy(sp => sp.SoldProperty.FirstLine).AsQueryable(),
                "sold_property_desc" => sorted.OrderByDescending(sp => sp.SoldProperty.FirstLine).AsQueryable(),
                "buyer_desc" => sorted.OrderByDescending(b => b.Buyer.FirstName).AsQueryable(),
                _ => sorted.OrderBy(b => b.Buyer.FirstName).AsQueryable(),
            };
            return sorted;
        }

        // GET: Staff/Sale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id<=0 )
            {
                _logger.LogError($"{id} is a invalid Id the was past to the Sale Detail Method ");
                return RedirectToAction(nameof(Index));
            }

            var sale = _mapper.Map<SaleManagementDTO>(await _repositoryWrapper.Sale.GetSaleByIdAsync(id));
           
            return View(nameof(Details),sale);
        }

        // GET: Staff/Sale/Create
        public async Task<IActionResult> Create()
        {

            ViewData["Addresses"] = await GetAddresses(null);

            return View(nameof(Create));
        }

        private async Task<List<SelectListItem>> GetAddresses(int? id)
        {
            List<Address> addressEntities = await _repositoryWrapper.Address.GetAllAddressesAsync();
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

        // POST: Staff/Sale/Create
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleManagementDTO sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    sale.AgentId = await _repositoryWrapper.Employee.GetUserId(User);
                    UpdateListing(sale);
                    _repositoryWrapper.Sale.CreateSale(_mapper.Map<Sale>(sale));
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Error creating Sale: {ex}");
                }
          
                return RedirectToAction(nameof(Index));
            }
          
            ViewData["Addresses"] = await GetAddresses(null);
            return View(nameof(Create), sale);
        }

        private void UpdateListing(SaleManagementDTO sale)
        {
            Listing listing = _repositoryWrapper.Listing.GetListingsIdByAddressID(sale.PropertyId);
            listing.Status = sale.Status;
            _repositoryWrapper.Listing.UpdateListing(listing);
        }

        // GET: Staff/Sale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id<=0)
            {
                _logger.LogError($"{id} passed to the Sale Edit method is invalid");
                return RedirectToAction(nameof(Index));
            }
           
                var sale = _mapper.Map<SaleManagementDTO>(await _repositoryWrapper.Sale.GetSaleByIdAsync(id));


           ViewData["Addresses"] = await GetAddresses(null);
            return View(nameof(Edit),sale);
        }

        // POST: Staff/Sale/Edit/5
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SaleManagementDTO sale)
        {
            if(id != sale.Id)

            {
                _logger.LogError($"Id did not match for to {nameof(Edit)}.  Expected:{id}, Actual{sale.Id}");
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //todo set the sale date.
                    //todo change the status of the property that has been sold or moved to under contract.
                    UpdateListing(sale);
                    _repositoryWrapper.Sale.UpdateSale(_mapper.Map<Sale>(sale));
                    await _repositoryWrapper.SaveAsync();
                } catch(Exception ex)
                {
                    _logger.LogError($"Error creating Sale: {ex}");
                }

                return RedirectToAction(nameof(Index));
            }
          
            ViewData["Addresses"] = await GetAddresses(null);
            return View(nameof(Edit), sale);
        }

        // GET: Staff/Sale/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"{id} was passed to the Sale delete method and was invalid");
                return RedirectToAction(nameof(Index));
            }

            var sale = _mapper.Map<SaleManagementDTO>(await _repositoryWrapper.Sale.GetSaleByIdAsync(id));
           

            return View(nameof(Delete),sale);
        }

        // POST: Staff/Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sale = await _repositoryWrapper.Sale.GetSaleByIdAsync(id);
                _repositoryWrapper.Sale.DeleteSale(sale);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
              
            catch (Exception ex)
            {
                _logger.LogError($"unable to delete sale: {ex}");
                return RedirectToAction(nameof(Delete), id);
    }
}

    }
}
