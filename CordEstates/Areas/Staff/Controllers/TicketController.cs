using AutoMapper;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Models;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class TicketController : Controller
    {


        readonly IMapper _mapper;
        readonly IRepositoryWrapper _repositoryWrapper;
        readonly ILoggerManager _logger;

        public TicketController(ILoggerManager loggerManager, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _logger = loggerManager;
            _repositoryWrapper = repositoryWrapper;

            _mapper = mapper;
        }

        // GET: Admin/Ticket
        public async Task<IActionResult> Index(string sortOrder,int pageNumber =1)
        {
            var data = _mapper.Map<List<TicketManagementDTO>>(await _repositoryWrapper.Ticket.GetAllTicketsAsync());
            IQueryable<TicketManagementDTO> sorted = data.AsQueryable();


            ViewData["FirstNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "Last Name" ? "last_name_desc" : "Last Name";
            ViewData["CreationDateSortParm"] = sortOrder == "Creation Date" ? "creation_date_desc" : "Creation Date";
            ViewData["ActionedSortParm"] = sortOrder == "Actioned" ? "actioned_desc" : "Actioned";
            ViewData["currentSort"] = sortOrder;

            sorted = SortList(sortOrder, sorted);

            var model = PaginatedList<TicketManagementDTO>.Create(sorted, pageNumber, 5);
            return View(nameof(Index), model);
        }

        private static IQueryable<TicketManagementDTO> SortList(string sortOrder, IQueryable<TicketManagementDTO> sorted)
        {
            switch (sortOrder)
            {
                case "Last Name":
                    sorted = sorted.OrderBy(ln => ln.LastName).AsQueryable();
                    break;
                case "last_name_desc":
                    sorted = sorted.OrderByDescending(l => l.LastName).AsQueryable();
                    break;
                case "Creation Date":
                    sorted = sorted.OrderBy(c => c.SentAt).AsQueryable();
                    break;
                case "creation_date_desc":
                    sorted = sorted.OrderByDescending(c => c.SentAt).AsQueryable();
                    break;
               

                case "Actioned":
                    sorted = sorted.OrderBy(a => a.Actioned).AsQueryable();
                    break;
                case "actioned_desc":
                    sorted = sorted.OrderByDescending(a =>a.Actioned).AsQueryable();
                    break;
                case "first_name_desc":
                    sorted = sorted.OrderByDescending(f => f.FirstName).AsQueryable();
                    break;
                default:
                    sorted = sorted.OrderBy(f => f.FirstName).AsQueryable();
                    break;
            }

            return sorted;
        }

        // GET: Admin/Ticket/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var ticket = _mapper.Map<TicketManagementDTO>(await _repositoryWrapper.Ticket.GetTicketByIdAsync(id));


            return View(nameof(Details), ticket);
        }



        // GET: Admin/Ticket/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var ticket = _mapper.Map<TicketManagementDTO>(await _repositoryWrapper.Ticket.GetTicketByIdAsync(id));

            return View(nameof(Edit), ticket);
        }

        // POST: Admin/Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TicketManagementDTO ticket)
        {
            if (id != ticket.Id)
            {
                _logger.LogError($"Id did not match when editing Ticket: Expected id was - {id}, Received id was -{ticket.Id}");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Ticket ticketItem = _mapper.Map<Ticket>(ticket);
                    _repositoryWrapper.Ticket.UpdateTicket(ticketItem);

                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Concurrency Error when editing event : {ticket.Id}; EXCEPTION {ex}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), ticket);
        }





    }
}
