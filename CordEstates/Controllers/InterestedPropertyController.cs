using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Areas.Identity.Data;
using CordEstates.Models.DTOs;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using AutoMapper;
using CordEstates.Entities;

namespace CordEstates.Controllers
{
    public class InterestedPropertyController : Controller
    {
        private readonly ILoggerManager _logger;
        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;

        public InterestedPropertyController(ILoggerManager logger, IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;

        }

        // GET: InterestedProperty
        public async Task<IActionResult> Index()
        {
            string user = await _repositoryWrapper.Employee.GetUserId(User);
            var customer = await _repositoryWrapper.Customer.GetCustomerByUserId(user);
            CustomerListingDTO customerListing = _mapper.Map<CustomerListingDTO>(await _repositoryWrapper.Customer.GetCustomersPropertyAsync(customer.Id));
            return View(nameof(Index),customerListing);
            //todo change what the index is displaying
            //todo redirect the details page to the listing controller details
        }
               

    

    
    }
}
