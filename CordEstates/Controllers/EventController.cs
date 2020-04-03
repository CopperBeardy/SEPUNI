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
    public class EventController : Controller
    {

        readonly IMapper _mapper;
        readonly ILoggerManager _logger;
        readonly IRepositoryWrapper _repositoryWrapper;
        public EventController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {

            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            //todo - possible enable customer to register interest.
            List<EventDTO> events = new List<EventDTO>();

            try
            {
                events = _mapper.Map<List<EventDTO>>(await _repositoryWrapper.Event.GetAllEventsAsync());
                return View(nameof(Index), events);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem occurred retrieving events: {ex}");
                throw new Exception($"Problem occurred retrieving events: {ex}");
            }
        }


    }
}
