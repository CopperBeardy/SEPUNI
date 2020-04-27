using AutoMapper;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models;
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


    public class EventController : Controller
    {
        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly IHostEnvironment _hostEnvironment;
        readonly ILoggerManager _logger;
        readonly IImageUploadWrapper _imageUploadWrapper;
        public EventController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper,
            IHostEnvironment hostEnvironment, IImageUploadWrapper imageUploadWrapper)
        {
            _imageUploadWrapper = imageUploadWrapper;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Staff/Event
        public async Task<IActionResult> Index(int pageNumber =1)
        {
            List<EventManagementDTO> data = _mapper.Map<List<EventManagementDTO>>(await _repositoryWrapper.Event.GetAllEventsAsync()).ToList();
            IQueryable<EventManagementDTO> dataQuerable = data.AsQueryable();
            var model = PaginatedList<EventManagementDTO>.Create(dataQuerable, pageNumber, 5);
            return View(nameof(Index), model);
        }

        // GET: Staff/Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                _logger.LogError($"invalid value passed to Event controller, method : {nameof(Delete)}");
                return RedirectToAction(nameof(Index));

            }

            var eventDetail = _mapper.Map<EventManagementDTO>(await _repositoryWrapper.Event.GetEventByIdAsync(id));


            return View(nameof(Details), eventDetail);
        }

        // GET: Admin/Event/Create
        public IActionResult Create()
        {

            return View(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventManagementDTO eventDTO)
        {
            if (ModelState.IsValid)
            {
                eventDTO.Photo = new Photo()
                {
                    ImageLink = _imageUploadWrapper.Upload(eventDTO.File, _hostEnvironment)
                };

                _repositoryWrapper.Event.CreateEvent(_mapper.Map<Event>(eventDTO));
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Create), eventDTO);
        }

        // GET: Admin/Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var eventEdit = _mapper.Map<EventManagementDTO>(await _repositoryWrapper.Event.GetEventByIdAsync(id));


            return View(nameof(Edit), eventEdit);
        }

        // POST: Admin/Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventManagementDTO eventEdit)
        {
            if (id != eventEdit.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (eventEdit.File != null)
                    {
                        eventEdit.Photo = new Photo()
                        {
                            ImageLink = _imageUploadWrapper.Upload(eventEdit.File, _hostEnvironment)
                        };
                    }

                    var eventItem = _mapper.Map<Event>(eventEdit);
                    _repositoryWrapper.Event.UpdateEvent(eventItem);
                    await _repositoryWrapper.SaveAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Concurrency Error when editing event : {eventEdit.Id}; EXCEPTION {ex}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), eventEdit);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var eventEdit = _mapper.Map<EventManagementDTO>(await _repositoryWrapper.Event.GetEventByIdAsync(id));

            return View(nameof(Delete), eventEdit);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var eventItem = await _repositoryWrapper.Event.GetEventByIdAsync(id);
                _repositoryWrapper.Event.DeleteEvent(eventItem);
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
