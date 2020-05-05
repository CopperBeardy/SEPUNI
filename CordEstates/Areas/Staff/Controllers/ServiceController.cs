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
    public class ServiceController : Controller
    {

        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;


        public ServiceController(ILoggerManager loggerManager, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = loggerManager;
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;

        }

        // GET: Employees/Service
        public async Task<IActionResult> Index(string sortOrder,int pageNumber=1)
        {
            List<ServiceDTO> data = _mapper.Map<List<ServiceDTO>>( await _repositoryWrapper.Service.GetAllServicesAsync());
            IQueryable<ServiceDTO> sorted = data.AsQueryable();
            ViewData["ServiceNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "service_name_desc" : "";          
            ViewData["currentSort"] = sortOrder;

            sorted = SortList(sortOrder, sorted);
            var model = PaginatedList<ServiceDTO>.Create(sorted, pageNumber, 5);
            return View(nameof(Index), model);
        }
        private static IQueryable<ServiceDTO> SortList(string sortOrder, IQueryable<ServiceDTO> sorted)
        {
            sorted = sortOrder switch
            {
                "service_name_desc" => sorted.OrderByDescending(s => s.ServiceName).AsQueryable(),
                _ => sorted.OrderBy(s => s.ServiceName).AsQueryable(),
            };
            return sorted;
        }


        // GET: Employees/Service/Create
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceDTO serviceDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _mapper.Map<Service>(serviceDTO);
                _repositoryWrapper.Service.CreateService(result);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Create), serviceDTO);
        }

        // GET: Employees/Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"{id} was passed to Staff.ServiceController.Edit _get_");
                return RedirectToAction(nameof(Index));
            }

            var serviceDTO = _mapper.Map<ServiceDTO>(await _repositoryWrapper.Service.GetServiceByIdAsync(id));

            return View(nameof(Edit), serviceDTO);
        }

        // POST: Employees/Service/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceDTO serviceDTO)
        {
            if (id != serviceDTO.Id)
            {
                _logger.LogError($"Id do not match for Staff.Service.Edit. Expected: {id},Actual {serviceDTO} _post_");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _mapper.Map<Service>(serviceDTO);
                    _repositoryWrapper.Service.UpdateService(result);
                    await _repositoryWrapper.SaveAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred whilst attempting to update Service with id- {id}: {ex}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), serviceDTO);

        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"{id} is a invalid id that was passed to the Staff.ServiceController.Delete");
                return RedirectToAction(nameof(Index));
            }

            var service = _mapper.Map<ServiceDTO>(await _repositoryWrapper.Service.GetServiceByIdAsync(id));


            return View(nameof(Delete), service);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Service service = await _repositoryWrapper.Service.GetServiceByIdAsync(id);
                _repositoryWrapper.Service.DeleteService(service);
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"unable to delete Event: {ex}");
                return RedirectToAction(nameof(Delete), id);
            }
        }


    }
}
