using AutoMapper;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var tickets = _mapper.Map<List<TicketManagementDTO>>(await _repositoryWrapper.Ticket.GetAllTicketsAsync());
            
            return View(nameof(Index), tickets);
        }

        // GET: Admin/Ticket/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var ticket = _mapper.Map<TicketManagementDTO>(await _repositoryWrapper.Ticket.GetTicketByIdAsync(id));
           

            return View(nameof(Details),ticket);
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
            return View(nameof(Edit),ticket);
        }



        

    }
}
