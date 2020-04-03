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
    public class ServiceController : Controller
    {


        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public ServiceController(ILoggerManager logger, IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var services = new List<ServiceDTO>();
            try
            {
                services = _mapper.Map<List<ServiceDTO>>(await _repositoryWrapper.Service.GetAllServicesAsync());
                return View(nameof(Index), services);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem has occurred on loading services: {ex}");
                throw new Exception($"problem has occured loading services {ex}");
            }



        }
    }
}