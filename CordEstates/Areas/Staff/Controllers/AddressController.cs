using AutoMapper;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models;
using CordEstates.Models.DTOs;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff, Admin")]
    public class AddressController : Controller
    {



        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public AddressController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Employees/Address
        public async Task<IActionResult> Index(string sortOrder,int pageNumber =1)
        {
            var data = _mapper.Map<List<AddressDTO>>(await _repositoryWrapper.Address.GetAllAddressesAsync());
           
            
            IQueryable<AddressDTO> sorted = data.AsQueryable();
         

            ViewData["NumSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewData["FirstLineSortParm"] = sortOrder== "First Line" ? "first_line_desc" : "First Line";
            ViewData["SecondLineSortParm"] = sortOrder == "Second Line" ? "second_line_desc" : "Second Line";
            ViewData["TownCitySortParm"] = sortOrder == "Town/City" ? "town_city_desc" : "Town/City";
            ViewData["PostcodeSortParm"] = sortOrder == "Postcode" ? "postcode_desc" : "Postcode";
            ViewData["currentSort"] = sortOrder;
            sorted = SortList(sortOrder, sorted);
   PaginatedList<AddressDTO> model = PaginatedList<AddressDTO>.Create(sorted, pageNumber, 5);
            
            return View(nameof(Index),model);

        }

        private static IQueryable<AddressDTO> SortList(string sortOrder, IQueryable<AddressDTO> sorted)
        {
            switch (sortOrder)
            {
                case "First Line":
                    sorted = sorted.OrderBy(f => f.FirstLine).AsQueryable();
                    break;
                case "first_line_desc":
                    sorted = sorted.OrderByDescending(fl => fl.FirstLine).AsQueryable();
                    break;
                case "second_line_desc":
                    sorted = sorted.OrderByDescending(sl => sl.SecondLine).AsQueryable();
                    break;
                case "Second Name":
                    sorted = sorted.OrderBy(sl => sl.SecondLine).AsQueryable();
                    break;
                case "Town/City":
                    sorted = sorted.OrderBy(t => t.TownCity).AsQueryable();
                    break;
                case "town_city_desc":
                    sorted = sorted.OrderByDescending(t => t.TownCity).AsQueryable();
                    break;
                case "PostCode":
                    sorted = sorted.OrderBy(p => p.Postcode).AsQueryable();
                    break;
                case "postcode_desc":
                    sorted = sorted.OrderByDescending(p => p.Postcode).AsQueryable();
                    break;
                case "number_desc":
                    sorted = sorted.OrderByDescending(n => n.Number).AsQueryable();
                    break;
                default:
                    sorted = sorted.OrderBy(n => n.Number).AsQueryable();
                    break;
            }

            return sorted;
        }

        // GET: Employees/Address/Create
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        // POST: Employees/Address/Create
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressDTO addressDTO)
        {
            if (ModelState.IsValid)
            {

                _repositoryWrapper.Address.CreateAddress(_mapper.Map<Address>(addressDTO));
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Create), addressDTO);
        }

        // GET: Employees/Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"Null passed to Staff.Address.Edit");
                return RedirectToAction(nameof(Index));
            }

            AddressDTO addressDTO = _mapper.Map<AddressDTO>(await _repositoryWrapper.Address.GetAddressByIdAsync(id));

            return View(nameof(Edit), addressDTO);
        }

        // POST: Employees/Address/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddressDTO addressDTO)
        {


            if (ModelState.IsValid)
            {

                Address address = _mapper.Map<Address>(addressDTO);
                _repositoryWrapper.Address.UpdateAddress(address);
                await _repositoryWrapper.SaveAsync();

            }
            return View(nameof(Edit), addressDTO);
        }


        [Authorize(Roles = " Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"Null passed to Staff.Address.Delete");
                return RedirectToAction(nameof(Index));
            }

            var address = _mapper.Map<AddressDTO>(await _repositoryWrapper.Address.GetAddressByIdAsync(id));


            return View(nameof(Delete), address);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                var address = await _repositoryWrapper.Address.GetAddressByIdAsync(id);
                _repositoryWrapper.Address.DeleteAddress(address);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"unable to delete address: {ex}");
                return RedirectToAction(nameof(Delete), id);
            }
        }



    }
}
