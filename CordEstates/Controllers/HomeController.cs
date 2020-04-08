using AutoMapper;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models.DTOs;
using CordEstates.Models.ViewModels;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;
        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;

        public HomeController(ILoggerManager logger, IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            LandingPageViewModel vm = new LandingPageViewModel();

            try
            {
                vm.Services = _mapper.Map<List<ServiceDTO>>(await _repositoryWrapper.Service.GetAllServicesAsync());
                vm.Listings = _mapper.Map<List<LandingListingDTO>>(await _repositoryWrapper.Listing.GetLandingPageListingsAsync(3));
                vm.Events = _mapper.Map<EventDTO>(await _repositoryWrapper.Event.GetActiveEventAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Index)} encounter a problem when trying to retrive all services from Database: {ex}");
                throw new Exception($"{nameof(Index)} encounter a problem when trying to retrive all services from Database { ex}");
            }
            // var role = User.IsInRole("Staff");

            return View(nameof(Index), vm);
        }


        public async Task<IActionResult> About()
        {
            List<UserDTO> staff = new List<UserDTO>();
            try
            {
                var mod = await _repositoryWrapper.User.GetAllStaffAsync();
                staff = _mapper.Map<List<UserDTO>>(mod);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(About)} encountered a error retrieving staff members from Database: {ex}");
                throw new Exception($"{nameof(About)} encountered a error retrieving staff members from  Database: {ex}");


            }


            return View(nameof(About), staff);
        }




        public IActionResult Contact()
        {
            return View(nameof(Contact), new CreateTicketDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Contact(CreateTicketDTO model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.Ticket.CreateTicket(_mapper.Map<Ticket>(model));

                }
                catch (Exception ex)
                {
                    _logger.LogError($"{nameof(Contact)} encountered a error adding Ticket to Database: {ex}");
                    throw new Exception($"{nameof(Contact)} encountered a error adding Ticket to Database: {ex}");

                }
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Contact), model);

        }






    }
}
