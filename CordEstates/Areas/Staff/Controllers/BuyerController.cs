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
        public async Task<IActionResult> Index(int pageNumber =1)
        {
            var data = _mapper.Map<List<BuyerManagementDTO>>(await _repositoryWrapper.Buyer.GetAllBuyersAsync());
            IQueryable<BuyerManagementDTO> dataQuerable = data.AsQueryable();
            var model = PaginatedList<BuyerManagementDTO>.Create(dataQuerable, pageNumber, 5);
            return View(nameof(Index),model);
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
