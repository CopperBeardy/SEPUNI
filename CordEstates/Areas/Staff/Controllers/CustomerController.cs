﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CordEstates.Areas.Identity.Data;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using AutoMapper;
using System.Reflection.Metadata;
using CordEstates.Entities;
using CordEstates.Models;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class CustomerController : Controller
    {
        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;
        public CustomerController(ILoggerManager logger, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        // GET: Staff/Customer
        public async Task<IActionResult> Index(string sortOrder,int pageNumber =1)
        {
            List<CustomerManagementDTO> data = _mapper.Map<List<CustomerManagementDTO>>(await _repositoryWrapper.Customer.GetAllCustomersAsync());

            ViewData["FirstNameSortParm"] = sortOrder == "First Name" ? "first_name_desc" : "First Name";
            ViewData["LastNameSortParm"] = sortOrder == "Last Name" ? "last_name_desc" : "Last Name";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["currentSort"] = sortOrder;
            IQueryable<CustomerManagementDTO> sorted = data.AsQueryable();
            sorted = SortList(sortOrder, sorted);
            PaginatedList<CustomerManagementDTO> model = PaginatedList<CustomerManagementDTO>.Create(sorted, pageNumber, 5);


            return View(model);
        }

        private static IQueryable<CustomerManagementDTO> SortList(string sortOrder, IQueryable<CustomerManagementDTO> sorted)
        {
            sorted = sortOrder switch
            {
                "First Name" => sorted.OrderBy(f => f.User.FirstName).AsQueryable(),
                "first_name_desc" => sorted.OrderByDescending(f => f.User.FirstName).AsQueryable(),
                "Last Name" => sorted.OrderBy(ln => ln.User.LastName).AsQueryable(),
                "last_name_desc" => sorted.OrderByDescending(l => l.User.LastName).AsQueryable(),
                "Email" => sorted.OrderBy(e => e.User.Email).AsQueryable(),
                "email_desc" => sorted.OrderByDescending(e => e.User.Email).AsQueryable(),
                "id_desc" => sorted.OrderByDescending(e => e.Id).AsQueryable(),
                _ => sorted.OrderBy(i => i.Id).AsQueryable(),
            };
            return sorted;
        }



        // GET: Staff/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerManagementDTO = _mapper.Map<CustomerManagementDTO>(await _repositoryWrapper.Customer.GetCustomerByIdAsync(id));
      
            return View(customerManagementDTO);
        }

        // GET: Staff/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staff/Customer/Create
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CustomerManagementDTO customerManagementDTO)
        {
            if (ModelState.IsValid)
            {
                 _repositoryWrapper.Customer.CreateCustomer(_mapper.Map<Customer>(customerManagementDTO));
                await _repositoryWrapper.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerManagementDTO);
        }

        // GET: Staff/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerManagementDTO = _mapper.Map<CustomerManagementDTO>(await _repositoryWrapper.Customer.GetCustomerByIdAsync(id));

            return View(customerManagementDTO);
        }

        // POST: Staff/Customer/Edit/5
        // To protect from over posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,FirstName,LastName,Email")] CustomerManagementDTO customerManagementDTO)
        {
            if (id != customerManagementDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repositoryWrapper.Customer.UpdateCustomer(_mapper.Map<Customer>(customerManagementDTO));
                    await _repositoryWrapper.SaveAsync();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"error occurred when updating customer entity : {customerManagementDTO.Id}. stack trace - {ex}");
                    return View(customerManagementDTO);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customerManagementDTO);
        }

        // GET: Staff/Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerManagementDTO = _mapper.Map<CustomerManagementDTO>(await _repositoryWrapper.Customer.GetCustomerByIdAsync(id));

            if (customerManagementDTO == null)
            {
                return NotFound();
            }

            return View(customerManagementDTO);
        }

        // POST: Staff/Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerManagementDTO = await _repositoryWrapper.Customer.GetCustomerByIdAsync(id);

            _repositoryWrapper.Customer.DeleteCustomer(customerManagementDTO);
            await _repositoryWrapper.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
