using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using CordEstates.Wrappers.Interfaces;
using CordEstates.Helpers;
using CordEstates.Areas.Staff.Models.DTOs;

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
        public async Task<IActionResult> Index()
        {
            var result = _mapper.Map<List<SaleManagementDTO>>(await _repositoryWrapper.Sale.GetAllSalesAsync());
       
            return View(nameof(Index),result);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleManagementDTO sale)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sale.AgentId = await _repositoryWrapper.User.GetUserId(User); 
                    _repositoryWrapper.Sale.CreateSale(_mapper.Map<Sale>(sale));
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Error creating Sale: {ex}");
                }
          
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "FirstLine", sale.BuyerId);

            ViewBag["Addresses"] = await GetAddresses(null);
            return View(nameof(Create), sale);
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



            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "FirstLine", sale.BuyerId);
            //ViewData["AgentId"] = new SelectList(_context.Users, "Id", "Id", sale.AgentId);
            //ViewData["PropertyId"] = new SelectList(_context.Addresses, "Id", "FirstLine", sale.PropertyId);
            ViewBag["Addresses"] = await GetAddresses(null);
            return View(nameof(Edit),sale);
        }

        // POST: Staff/Sale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
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
                    _repositoryWrapper.Sale.UpdateSale(_mapper.Map<Sale>(sale));
                    await _repositoryWrapper.SaveAsync();
                } catch(Exception ex)
                {
                    _logger.LogError($"Error creating Sale: {ex}");
                }

                return RedirectToAction(nameof(Index));
            }
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "FirstLine", sale.BuyerId);

            ViewBag["Addresses"] = await GetAddresses(null);
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
            var sale = await _repositoryWrapper.Sale.GetSaleByIdAsync(id);
            _repositoryWrapper.Sale.DeleteSale(sale);
            await _repositoryWrapper.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
